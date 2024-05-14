#version 330 core

in vec3 glPosition;
out vec4 FragColor;


struct SCamera
{
	vec3 Position;
	vec3 View;
	vec3 Up;
	vec3 Side;
	vec2 Scale;
};

struct SRay
{
	vec3 Origin;
	vec3 Direction;
};


SRay GenerateRay ( SCamera uCamera )
{
	vec2 coords = glPosition.xy * uCamera.Scale;
	vec3 direction = uCamera.View + uCamera.Side * coords.x + uCamera.Up * coords.y;
	return SRay ( uCamera.Position, normalize(direction) );
}

SCamera initializeDefaultCamera()
{
	SCamera camera;
	camera.Position = vec3(0.0, 0.0, -8.0);
	camera.View = vec3(0.0, 0.0, 1.0);
	camera.Up = vec3(0.0, 1.0, 0.0);
	camera.Side = vec3(1.0, 0.0, 0.0);
	// can not work
	camera.Scale = vec2(1.0);
	return camera;
}


void main()
{
	SCamera uCamera = initializeDefaultCamera();
	SRay ray = GenerateRay( uCamera);
	FragColor = vec4 ( abs(ray.Direction.xy), 0, 1.0);
}