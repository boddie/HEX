using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Playerscript : Photon.MonoBehaviour
{
    private AudioSource _walking;
    private AudioSource _hit;

    private Animator _animator;
	private Transform playerFollow;
	
	private Vector3 newPosition;
	private Quaternion newRotation;
	
	private bool firstSend = true;

    private Quaternion _forward;
    private Quaternion _forwardright;
    private Quaternion _backward;
    private Quaternion _backwardleft;
    private Quaternion _left;
    private Quaternion _forwardleft;
    private Quaternion _right;
    private Quaternion _backwardright;

    private Vector3 _forwardv;
    private Vector3 _forwardrightv;
    private Vector3 _backwardv;
    private Vector3 _backwardleftv;
    private Vector3 _leftv;
    private Vector3 _forwardleftv;
    private Vector3 _rightv;
    private Vector3 _backwardrightv;

    private float speed = .4f;
	private int speedMultiplier = 50;
	
	private float respawnTimer;
	private bool spawning = false;
	private float respawnMultiplier = 2;
	private bool dead = false;
    private bool gameOver = false;

    private Camera cam;
	
	private Vector3 initialPosition;
	
	protected PersistentData manager;

    private Vector3 _prevPos;
    private Vector3 moveDirection;

    private Vector3 _lastMousePosition1;
    private Vector3 _lastMousePosition2;

    private ScreenPad touchPad;

    private enum Mations
    {
        IDLE,
        RUNNING,
        DEAD
    }

    private Mations _myAnimations = Mations.IDLE;
	
	void Start()
	{
        var audioSources = this.GetComponents<AudioSource>();
        _walking = audioSources[0];
        _hit = audioSources[1];
        _walking.loop = true;
        _hit.loop = false;
        _animator = GetComponent<Animator>();
		playerFollow = GameObject.Find("PlayerTracer").GetComponent<Transform>();
        manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
        touchPad = GameObject.Find("TouchMovement").GetComponent<User>().pad;
        if (photonView.isMine)
        {
			initialPosition = transform.position;
            playerFollow.transform.rotation = this.transform.rotation;

            Vector3 unity = new Vector3(0.0f, 1.0f, 0.0f);

            _forward = transform.rotation;
            _backward = _forward * Quaternion.AngleAxis(180.0f, unity);
            _right = _forward * Quaternion.AngleAxis(90.0f, unity);
            _left = _forward * Quaternion.AngleAxis(-90.0f, unity);
            _forwardright = _forward * Quaternion.AngleAxis(45.0f, unity);
            _forwardleft = _forward * Quaternion.AngleAxis(-45.0f, unity);
            _backwardleft = _forward * Quaternion.AngleAxis(-135.0f, unity);
            _backwardright = _forward * Quaternion.AngleAxis(135.0f, unity);

            _forwardv = _forward * (speed * Vector3.forward);
            _backwardv = _backward * (speed * Vector3.forward);
            _rightv = _right * (speed * Vector3.forward);
            _leftv = _left * (speed * Vector3.forward);
            _forwardrightv = _forwardright * (speed * Vector3.forward);
            _forwardleftv = _forwardleft * (speed * Vector3.forward);
            _backwardrightv = _backwardright * (speed * Vector3.forward);
            _backwardleftv = _backwardleft * (speed * Vector3.forward);
           
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
			
			
			manager.LastLife = false;
        }
        _prevPos = this.transform.position;
	}

    void Update()
    {
        if (photonView.isMine)
        {          
            if (!gameOver)
            {
                if (manager.isLastManStanding(manager.CurrentPlayer.Alias))
                {
                    gameOver = true;
                    manager.eliminate(manager.CurrentPlayer.Alias);
                }
                manager.CurrentPlayer.Position = transform.position;
                if (manager.CurrentPlayer.ChangePositionTo != Vector3.zero)
                {
                    transform.position = manager.CurrentPlayer.ChangePositionTo;
                    manager.CurrentPlayer.ChangePositionTo = Vector3.zero;
                    manager.CurrentPlayer.Position = transform.position; //this is in case other movements are occuring at the same time
                }

                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

                //if we're in "Run" mode, respond to input for jump, and set the Jump parameter accordingly. 
                if (stateInfo.nameHash == Animator.StringToHash("Base Layer.Idle") && moveDirection != Vector3.zero)
                {
                    if (!_walking.isPlaying)
                        _walking.Play();
                    _animator.SetBool("Running", true);
                    _animator.Play(Animator.StringToHash("Base Layer.Locomotion"));
                    photonView.RPC("setPlayerAnimation", PhotonTargets.Others, (int)Mations.RUNNING);
                }
                else if (stateInfo.nameHash == Animator.StringToHash("Base Layer.Locomotion") && moveDirection == Vector3.zero)
                {
                    if (_walking.isPlaying)
                        _walking.Stop();
                    _animator.SetBool("Running", false);
                    _animator.Play(Animator.StringToHash("Base Layer.Idle"));
                    photonView.RPC("setPlayerAnimation", PhotonTargets.Others, (int)Mations.IDLE);
                }
                else if ((stateInfo.nameHash == Animator.StringToHash("Base Layer.Locomotion") ||
                    stateInfo.nameHash == Animator.StringToHash("Base Layer.Idle")) && (dead || spawning))
                {
                    if (_walking.isPlaying)
                        _walking.Stop();
                    _animator.SetBool("Dead", true);
                    _animator.Play(Animator.StringToHash("Base Layer.Death"));
                    photonView.RPC("setPlayerAnimation", PhotonTargets.Others, (int)Mations.DEAD);
                }
                else if (stateInfo.nameHash == Animator.StringToHash("Base Layer.Death") && !spawning)
                {
                    if (_walking.isPlaying)
                        _walking.Stop();
                    _animator.SetBool("Dead", false);
                    _animator.Play(Animator.StringToHash("Base Layer.Idle"));
                    photonView.RPC("setPlayerAnimation", PhotonTargets.Others, (int)Mations.IDLE);
                }

                if (manager.CurrentPlayer.CurrentHealth <= 0)
                {
                    if (manager.LastLife && !dead)
                    {
                        dead = true; // is this needed?
                        // End match
                        // Need to call next screen, but I don't think it should happen here..

                        gameOver = true;
                        manager.eliminate(manager.CurrentPlayer.Alias);
                        manager.CurrentPlayer.PlayerDied();
                        string killer = manager.CurrentPlayer.LastDamagedBy;
                        manager.giveKill(killer);
                        //PhotonNetwork.DestroyAll();

                        manager.screenManager.ActiveScreen = new Screen_MatchEndOverlay();
                    }
                    else if (!spawning && !dead)
                    {
                        spawning = true;
                        respawnTimer = Time.time + respawnMultiplier;
                        respawnMultiplier *= 2;

                        manager.CurrentPlayer.PlayerDied();
                        string killer = manager.CurrentPlayer.LastDamagedBy;
                        manager.giveKill(killer);
                    }
                    else if (respawnTimer < Time.time && !dead)
                    {
                        spawning = false;
                        transform.position = initialPosition;
                        transform.rotation = _forward;
                        manager.CurrentPlayer.Resurrect();
                        manager.respawned();
                    }
                }

                if (!spawning && !dead)
                {
                    if (!manager.CurrentPlayer.Immobilized)
                    {
                        float HInput = Input.GetAxis("Horizontal") * 15;
                        float VInput = Input.GetAxis("Vertical") * 15;

                        moveDirection = Vector3.zero;
                        if (VInput < 0 && HInput < 0 || touchPad.JoyDirection == StickDirection.DOWN_LEFT)
                        {
                            transform.rotation = _backwardleft;
                            moveDirection = _backwardleftv;
                        }
                        else if (VInput < 0 && HInput > 0 || touchPad.JoyDirection == StickDirection.DOWN_RIGHT)
                        {
                            transform.rotation = _backwardright;
                            moveDirection = _backwardrightv;
                        }
                        else if (VInput > 0 && HInput < 0 || touchPad.JoyDirection == StickDirection.UP_LEFT)
                        {
                            transform.rotation = _forwardleft;
                            moveDirection = _forwardleftv;
                        }
                        else if (VInput > 0 && HInput > 0 || touchPad.JoyDirection == StickDirection.UP_RIGHT)
                        {
                            transform.rotation = _forwardright;
                            moveDirection = _forwardrightv;
                        }
                        else if (VInput < 0 || touchPad.JoyDirection == StickDirection.DOWN)
                        {
                            transform.rotation = _backward;
                            moveDirection = _backwardv;
                        }
                        else if (VInput > 0 || touchPad.JoyDirection == StickDirection.UP)
                        {
                            transform.rotation = _forward;
                            moveDirection = _forwardv;
                        }
                        else if (HInput > 0 || touchPad.JoyDirection == StickDirection.RIGHT)
                        {
                            transform.rotation = _right;
                            moveDirection = _rightv;
                        }
                        else if (HInput < 0 || touchPad.JoyDirection == StickDirection.LEFT)
                        {
                            transform.rotation = _left;
                            moveDirection = _leftv;
                        }
                        transform.Translate(moveDirection * Time.deltaTime * speedMultiplier * (float)manager.CurrentPlayer.MovementSpeed, Space.World);
                        playerFollow.transform.position = this.transform.position;
                    }

                    if (!manager.CurrentPlayer.Silenced)
                    {

                        if (Input.GetMouseButtonDown(0))
                        {
                            _lastMousePosition1 = Input.mousePosition;
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            Vector3 currentMousePosition = Input.mousePosition;

                            if (Vector3.Distance(currentMousePosition, _lastMousePosition1) > Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) * 0.05f)
                            {
                                Vector3 lastWorldPosition = MathHelper.GetWorldFromMousePos(cam, _lastMousePosition1);
                                Vector3 currentWorldPosition = MathHelper.GetWorldFromMousePos(cam, currentMousePosition);
                                manager.CurrentPlayer.UsePrimarySwipeAbility(new SwipeAbilityUseContext(new SwipeGesture(_lastMousePosition1, currentMousePosition, lastWorldPosition, currentWorldPosition), manager.CurrentPlayer, transform.position));
                            }
                            else
                            {
                                Vector3 currentWorldPosition = MathHelper.GetWorldFromMousePos(cam, currentMousePosition);
                                manager.CurrentPlayer.UsePrimaryTapAbility(new TapAbilityUseContext(new TapGesture(currentMousePosition, currentWorldPosition), manager.CurrentPlayer, transform.position));
                            }
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            _lastMousePosition2 = Input.mousePosition;
                        }
                        else if (Input.GetMouseButtonUp(1))
                        {
                            Vector3 currentMousePosition = Input.mousePosition;

                            if (Vector3.Distance(currentMousePosition, _lastMousePosition2) > Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) * 0.05f)
                            {
                                Vector3 lastWorldPosition = MathHelper.GetWorldFromMousePos(cam, _lastMousePosition2);
                                Vector3 currentWorldPosition = MathHelper.GetWorldFromMousePos(cam, currentMousePosition);
                                manager.CurrentPlayer.UseSecondarySwipeAbility(new SwipeAbilityUseContext(new SwipeGesture(_lastMousePosition2, currentMousePosition, lastWorldPosition, currentWorldPosition), manager.CurrentPlayer, transform.position));
                            }
                            else
                            {
                                Vector3 currentWorldPosition = MathHelper.GetWorldFromMousePos(cam, currentMousePosition);
                                manager.CurrentPlayer.UseSecondaryTapAbility(new TapAbilityUseContext(new TapGesture(currentMousePosition, currentWorldPosition), manager.CurrentPlayer, transform.position));
                            }
                        }
                    }
                    else if (manager.CurrentPlayer.BuffManager.HasPEffect(new Assets.Buffs.IceBeamUse()) && manager.CurrentPlayer.SecondaryAbilities[manager.CurrentPlayer.ActiveSecondary].TapAbility.GetType() == typeof(WintersChillTap))
                    {
                        //so they're silenced, but they're currently using ice beam. well they can still reactivate ice beam.
                        if (Input.GetMouseButtonUp(1))
                        {
                            Vector3 currentMousePosition = Input.mousePosition;
                            Vector3 currentWorldPosition = MathHelper.GetWorldFromMousePos(cam, currentMousePosition);
                            manager.CurrentPlayer.UseSecondaryTapAbility(new TapAbilityUseContext(new TapGesture(currentMousePosition, currentWorldPosition), manager.CurrentPlayer, transform.position));
                        }
                    }
                }
            }
            else if (gameOver)
            {
                // In this case we want the player to still be able to control the camera.
             //   manager.CurrentPlayer.CalculateMatchResults(manager.PlayersLeft+1);
                manager.screenManager.OverlayScreen = null;
                manager.Quit();
                Application.LoadLevel("IndividualMatchResults");
                manager.screenManager.ActiveScreen = new Screen_IndividualMatchResults();
            }
        }
		else if(!firstSend)
		{
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 5);
		}

        if (!photonView.isMine)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

           manager.Opponents[photonView.owner.name].Position = newPosition;
            
            //manager.Opponents[

            switch (_myAnimations)
            {
                case Mations.IDLE:
                    if (stateInfo.nameHash != Animator.StringToHash("Base Layer.Idle"))
                    {
                        if (_walking.isPlaying)
                            _walking.Stop();
                        _animator.SetBool("Running", false);
                        _animator.SetBool("Dead", false);
                        _animator.Play(Animator.StringToHash("Base Layer.Idle"));
                    }
                    break;
                case Mations.DEAD:
                    if (stateInfo.nameHash != Animator.StringToHash("Base Layer.Death"))
                    {
                        if (_walking.isPlaying)
                            _walking.Stop();
                        _animator.SetBool("Running", false);
                        _animator.SetBool("Dead", true);
                        _animator.Play(Animator.StringToHash("Base Layer.Death"));
                    }
                    break;
                case Mations.RUNNING:
                    if (stateInfo.nameHash != Animator.StringToHash("Base Layer.Locomotion"))
                    {
                        if (!_walking.isPlaying)
                            _walking.Play();
                        _animator.SetBool("Running", true);
                        _animator.SetBool("Dead", false);
                        _animator.Play(Animator.StringToHash("Base Layer.Locomotion"));
                    }
                    break;
            }
        }
        _prevPos = this.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ability")
        {
            _hit.Play();
        }
    }

    [RPC]
    private void setPlayerAnimation(int animation)
    {
        _myAnimations = (Mations)animation;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //Executed on the owner of this PhotonView; 
            //The server sends it's position over the network
            
            stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
        }
        else
        {
            //Executed on the others; 
            //receive a position and set the object to it

            newPosition = (Vector3)stream.ReceiveNext();
			newRotation = (Quaternion)stream.ReceiveNext();
			
			if(firstSend)
			{
				firstSend = false;
				transform.position = newPosition;
				transform.rotation = newRotation;
			}
        }
    }
}
