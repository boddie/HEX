NOTE: For best results, make sure mipmaps are disabled for all textures on the explosion shader, compression MUST be disabled for the noise texture.

This package contains a pyroclastic noise explosion shader and the associated resources.

USAGE
AUTOMATIC
Drag the Pyroclastic Puff prefab into the scene. Scaling it will change the size of the explosion, and it should move around with the sphere object.

MANUAL
Adding the Explosion shader onto an object will not necessarily immediately work. The sphere is rendered at the position defined in the advanced panel, using the radius that you set. If you attach the ExplosionMat script, these properties will be set according to the position and scale of the GameObject, and a unique copy of the material will be maintained.