Mesh Conforming Decals v1.1
Thanks for purchasing.
The decals are simple to use but I will explain how in detail here.
To use a decal start by taking the basic decal prefab and placing it in the world. In order for it to work it has to intersect a collider with a meshfilter. It does not work with skinned meshes or terrain.
Once your decal is positioned properly, use the rotation parameter to change the direction of the projection. You can see the direction of the projection from the black line and square gizmo. Do not modify the transform of the decal's rotation. It won't affect anything.
To change the size of the decal don't use the transform's scale, instead use the scale parameter.
Once it is positioned and the rotation is set you can press the reset button to activate the decal.
Done!

To make a new decal material it must use the standard cutout shader and the texture needs to have a 1 pixel thick transparent outline in order for it to appear properly. The alpha cutoff might need to be adjusted too depending on your texture.

There are two booleans on the decal script.
Preview in edit mode: enabling this makes the decal get reset whenever you move, scale, or rotate the decal. Resource intensive.
Reset on play: this will make the decal reset during the start function when in play mode. Usually not necessary and should be disabled if you have set up your decal in the editor so that it doesn't have to remake the decal unnecessarily.
Another variable you might not need to use is decalOffset. It changes how far the decal is away from the mesh underneath. Only change it if you are having problems with your decals zfighting with the mesh underneath.

If you want to use the decals at runtime any object that the decal will be placed on must have static batching disabled(In the top right next to the checkbox by Static there is a drop down menu. Deselect "Batching Static"). Anyways, just instantiate the decal, set transform.position, set the rotation variable(not the transform's), and execute ResetDecal() and the decal should be placed correctly.

If you have any more questions you can pm me on reddit. My username is ToastehBro. Or you can send me a message on Twitter @ToastehBro.
