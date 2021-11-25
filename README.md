# Marching-cubes

Implementation of the marching cubes algorithm in Unity3d. I utilized HLSL and Compute Buffers to run this algorithm on the gpu. I also used a very simple shader graph with gradient to simulate terrain-like effect. This makes it look like an actual procedural terrain.

Triangulation table from the first reference link was used in this implementation. 

# Preview 

![alt text](gifs/1.gif)
![alt text](gifs/2.gif)

# Literature
* http://paulbourke.net/geometry/polygonise/
* https://developer.nvidia.com/gpugems/gpugems3/part-i-geometry/chapter-1-generating-complex-procedural-terrains-using-gpu
* https://www.ronja-tutorials.com/post/027-layered-noise/
