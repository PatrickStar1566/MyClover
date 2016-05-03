using UnityEngine;
using System.Collections;

public class GridOverview : MonoBehaviour {

	public GameObject plane;
	public GameObject[] obj;

	public bool showMain = true;
	public bool showSub = false;
	public bool showObj = false;

	public int gridSizeX;
	public int gridSizeY;
	public int gridSizeZ;

	public float smallStep;
	public float largeStep;

	public int gridObjSizeX;
	public int gridObjSizeY;
	public int gridObjSizeZ;

	public float startX;
	public float startY;
	public float startZ;

	public float startObjX;
	public float startObjY;
	public float startObjZ;

	private float offsetY = 0f;
	private float scrollRate = 0.1f;
	private float lastScroll = 0f;

	private Material lineMaterial;

	private Color mainColor = new Color(0f, 1f, 0f, 1f);
	private Color subColor = new Color(0f, 0.5f, 0f, 1f);
	private Color objColor = new Color(1f, 0f, 0f, 1f);
	
	void CreateLineMaterial()
	{
		if ( !lineMaterial )
		{
			lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
				"subshader {Pass {" +
				"   Blend SrcAlpha OneMinusSrcalpha " +
				"   Zwrite Off Cull Off Fog{ Mode off }" +
				"   BindChannels {" +
				"   Bind \"vertex\", vertex Bind \"color\" , color } " +
				"} } } ");
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	void OnPostRender()
	{
		CreateLineMaterial();
		lineMaterial.SetPass(0);
		GL.Begin(GL.LINES);
		if ( showSub )
		{
			GL.Color(subColor);
			for (float j = 0; j <= gridSizeY; j += smallStep)
			{
				for (float i = 0; i <= gridSizeZ - startZ; i += smallStep)
				{
					GL.Vertex3(startX, j + offsetY, startZ);
					GL.Vertex3(gridSizeX, j + offsetY, startZ + i);
				}

				for (float i = 0; i <= gridSizeX - startX; i += smallStep)
				{
					GL.Vertex3(startX + i, j + offsetY, startZ);
					GL.Vertex3(startX + i, j + offsetY, gridSizeZ);
				}
			}

			for (float i = 0; i <= gridSizeZ - startZ; i += smallStep)
			{
				for (float k = 0; k <= gridSizeX - startX; k += smallStep)
				{
					GL.Vertex3(startX + k, startY + offsetY, startZ + i);
					GL.Vertex3(startX + k, gridSizeY + offsetY, startZ + i);
				}
			}
		}

		if ( showMain )
		{
			GL.Color(mainColor);
			for (float j = 0; j <= gridSizeY; j += largeStep)
			{
				for (float i = 0; i <= gridSizeZ - startZ; i += largeStep)
				{
					GL.Vertex3(startX, j + offsetY, startZ);
					GL.Vertex3(gridSizeX, j + offsetY, startZ + i);
				}

				for (float i = 0; i <= gridSizeX - startX; i += largeStep)
				{
					GL.Vertex3(startX + i, j + offsetY, startZ);
					GL.Vertex3(startX + i, j + offsetY, gridSizeZ);
				}
			}

			for (float i = 0; i <= gridSizeZ - startZ; i += largeStep)
			{
				for (float k = 0; k <= gridSizeX - startX; k += largeStep)
				{
					GL.Vertex3(startX + k, startY + offsetY, startZ + i);
					GL.Vertex3(startX + k, gridSizeY + offsetY, startZ + i);
				}
			}
		}
		GL.End();
	}
}
