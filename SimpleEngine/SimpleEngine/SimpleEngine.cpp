#include "olcConsoleGameEngine.h"

#define PI 3.14159

using namespace std;

struct vec3d {
	float x, y, z;
};

struct triangle {
	vec3d p[3];
};

struct mesh {
	vector<triangle> tris;
};

struct math4x4 {
	float m[4][4] = { 0 };
};

class olcEngine3D : public olcConsoleGameEngine
{
public:
	olcEngine3D()
	{
		m_sAppName = L"Example";
	}

private:
	mesh meshCube;
	math4x4 mathProj;

	void initMeshCube() {
		meshCube.tris = {

			// SOUTH
			{0.0f, 0.0f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 1.0f, 0.0f},
			{0.0f, 0.0f, 0.0f,   1.0f, 1.0f, 0.0f,   1.0f, 0.0f, 0.0f},

			// EAST
			{1.0f, 0.0f, 0.0f,   1.0f, 1.0f, 0.0f,   1.0f, 1.0f, 1.0f},
			{1.0f, 0.0f, 0.0f,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f, 1.0f},

			// NORTH
			{1.0f, 0.0f, 1.0f,   1.0f, 1.0f, 1.0f,   0.0f, 1.0f, 1.0f},
			{1.0f, 0.0f, 1.0f,   0.0f, 1.0f, 1.0f,   0.0f, 0.0f, 1.0f},

			// WEST
			{0.0f, 0.0f, 1.0f,   0.0f, 1.0f, 1.0f,   0.0f, 1.0f, 0.0f},
			{0.0f, 0.0f, 1.0f,   0.0f, 1.0f, 0.0f,   0.0f, 0.0f, 0.0f},

			// TOP
			{0.0f, 1.0f, 0.0f,   0.0f, 1.0f, 1.0f,   1.0f, 1.0f, 1.0f},
			{0.0f, 1.0f, 0.0f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, 0.0f},

			// BOTTOM
			{1.0f, 0.0f, 1.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, 0.0f},
			{1.0f, 0.0f, 1.0f,   0.0f, 0.0f, 0.0f,   1.0f, 0.0f, 0.0f},
		};
	}

	void initMatrixProjection()
	{
		float fNear = 0.1f;
		float fFar = 1000.0f;
		float fFov = 90.0f;
		float fAspectRatio = (float)ScreenHeight() / (float)ScreenWidth();
		float fFovRad = 1.0f / tanf(fFov * 0.5f / 180.0f * PI);

		mathProj.m[0][0] = fAspectRatio * fFovRad;
		mathProj.m[1][1] = fFovRad;
		mathProj.m[2][2] = fFar / (fFar - fNear);
		mathProj.m[3][2] = (-fFar * fNear) / (fFar - fNear);
		mathProj.m[2][3] = 1.0f;
		mathProj.m[3][3] = 0.0f;
	}

	void ScaleIntoView(triangle &triProjected)
	{
		triProjected.p[0].x += 1.0f; triProjected.p[0].y += 1.0f;
		triProjected.p[1].x += 1.0f; triProjected.p[1].y += 1.0f;
		triProjected.p[2].x += 1.0f; triProjected.p[2].y += 1.0f;

		triProjected.p[0].x *= 0.5f * (float)ScreenWidth();
		triProjected.p[0].y *= 0.5f * (float)ScreenHeight();
		triProjected.p[1].x *= 0.5f * (float)ScreenWidth();
		triProjected.p[1].y *= 0.5f * (float)ScreenHeight();
		triProjected.p[2].x *= 0.5f * (float)ScreenWidth();
		triProjected.p[2].y *= 0.5f * (float)ScreenHeight();
	}

public:
	bool OnUserCreate() override
	{
		initMeshCube();
		initMatrixProjection();

		return true;


	}
	void MultiplyMatrixVector(vec3d &i, vec3d &o, math4x4 &m) {
		o.x = i.x * m.m[0][0] + i.y * m.m[1][0] + i.z* m.m[2][0] + m.m[3][0];
		o.y = i.x * m.m[0][1] + i.y * m.m[1][1] + i.z* m.m[2][1] + m.m[3][1];
		o.z = i.x * m.m[0][2] + i.y * m.m[1][2] + i.z* m.m[2][2] + m.m[3][2];
		float w = i.x * m.m[0][3] + i.y * m.m[1][3] + i.z* m.m[2][3] + m.m[3][3];

		if (w != 0)
		{
			o.x /= w, o.y /= w, o.z /= w;
		}
	}

	bool OnUserUpdate(float fElapsedTime) override
	{
		Fill(0, 0, ScreenWidth(), ScreenHeight(), PIXEL_SOLID, FG_BLACK);
		for (auto tri : meshCube.tris) 
		{
			triangle triProjected, triTranslated;

			triTranslated = tri;
			triTranslated.p[0].z = tri.p[0].z + 3.0f;
			triTranslated.p[1].z = tri.p[1].z + 3.0f;
			triTranslated.p[2].z = tri.p[2].z + 3.0f;
		
			MultiplyMatrixVector(triTranslated.p[0], triProjected.p[0], mathProj);
			MultiplyMatrixVector(triTranslated.p[1], triProjected.p[1], mathProj);
			MultiplyMatrixVector(triTranslated.p[2], triProjected.p[2], mathProj);

			// Scale into view
			ScaleIntoView(triProjected);

			DrawTriangle(
				triProjected.p[0].x, triProjected.p[0].y,
				triProjected.p[1].x, triProjected.p[1].y,
				triProjected.p[2].x, triProjected.p[2].y,
				PIXEL_SOLID, FG_WHITE);
		}
		return true;
	}
	
};


int main()
{
	olcEngine3D demo;
	if (demo.ConstructConsole(256, 240, 4, 4))
		demo.Start();

	return 0;
}