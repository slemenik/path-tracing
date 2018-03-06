# Phisically based rendering - Path tracing
A homework project for course of Advanced computer graphics, Faculty of Computer and Information Science, University of Ljubljana, Slovenia

![pathtracer](https://user-images.githubusercontent.com/32905529/37046458-b7d2bbbe-2168-11e8-8f63-7cca985df299.PNG)


### 1 Introduction
The goal of this homework is to get familiar with path tracing and the use of different light sources and materials in physically based rendering. Your task is to extend the path tracing framework provided with the homework and implement a basic path tracing algorithm (3), spherical lights (4) and Lambertian material (5) within the framework. 

### 2 The path tracing framework
For implementation of the homework you may use the provided framework. The framework is developed in C# and includes basic math and support for easier implementation of a path tracer. It is loosely based on the PBRTv3 rendering framework [1] developed by authors of book Physically Based Rendering [2].

### 3 Implementation of Path tracing
Implement the main path tracing method in the framework (within PathTracer.cs), which uses Russian roulette for stopping and importance sampling for choosing ray directions.

### 4 Lights
Extend the framework with support for spherical light sources.The source of the light is the surface of a sphere positioned within the scene. The user must be able to set the radius of the sphere. Implement uniform sampling of light rays from the surface. An example of a light is already provided by the disk light (Disk.cs).

### 5 Materials
Implement Lambertian material in the framework. The template file is Lambertian.cs.

### 6 Outputs
The expected outputs of this homework are example renderings (images), displaying the implemented features. 

#### References
[1] Matt Pharr, Greg Humphreys, and Wenzel Jakob. PBRTv3, https://github.com/mmp/pbrt-v3
[2] Matt Pharr, Greg Humphreys, and Wenzel Jakob, Physically Based Rendering, Third Edition, 2016.
