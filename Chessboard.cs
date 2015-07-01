using UnityEngine;
using System.Collections;

public class Chessboard : MonoBehaviour {
	
	public int m_iSize = 10;
	GameObject[,] m_Grid;

	void KillAll()
	{
		for (int i = 0; i < m_iSize; i++)
			for (int j = 0; j < m_iSize; j++) 
			{
			SetAlive(i, j, false);//m_Grid [i, j].GetComponent<Renderer>().material.color = Color.white;
			}
	}

	void Toggle (int _iColumn, int _iRow)
	{
				if (m_Grid [_iColumn, _iRow].GetComponent<Renderer> ().material.color == Color.white)
			SetAlive (_iColumn, _iRow, true);//m_Grid[_iColumn, _iRow].GetComponent<Renderer>().material.color = Color.blue;

				else
			SetAlive (_iColumn, _iRow, false);//m_Grid[_iColumn, _iRow].GetComponent<Renderer>().material.color = Color.white;
	}

	void ToggleMousePos()
	{
		Vector3 MouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int iIndexX = (int)(MouseWorldPos.x);
		int iIndexY = (int)(MouseWorldPos.y);
		Toggle (iIndexX, iIndexY);
	}

	bool GetAlive (int _iColumn, int _iRow)
	{
		return (m_Grid[_iColumn, _iRow].GetComponent<Renderer>().material.color == Color.blue);
	}

	void SetAlive(int _iColumn, int _iRow, bool _bAlive)
	{
		if (_bAlive)
				m_Grid[_iColumn, _iRow].GetComponent<Renderer>().material.color = Color.blue;
		else
				m_Grid[_iColumn, _iRow].GetComponent<Renderer>().material.color = Color.white;
	}

	/*int GetAliveNeighbours (int _iColumn, int _iRow)
	{
		int iAliveNeighbours = 0;
		//nachbarn für feld 3,8 wäre column 2-4 und row 7-9 (im range 1)
		for (int iColumn = _iColumn -1; iColumn <= _iColumn +1; iColumn++)
		{
			for (int iRow = _iRow -1; iRow <= _iRow -1; iRow++)
			{
				if (iColumn == _iColumn && iRow == _iRow)
					continue;

				if (iColumn >= 0 && iColumn < m_iSize && iRow >= 0 && iRow < m_iSize &&
				    m_Grid[iColumn, iRow].GetComponent<Renderer>().material.color == Color.blue) //Check Range/Bounce
					iAliveNeighbours++;
			}
		}
		return iAliveNeighbours;		
	}*/

	
	// Use this for initialization
	void Start () 
	{
		m_Grid = new GameObject[m_iSize, m_iSize];
		
		for (int i = 0; i < m_iSize; i++)
		for (int j = 0; j < m_iSize; j++) 
		{
			GameObject kachel = GameObject.CreatePrimitive(PrimitiveType.Quad);
			kachel.name = "Kachel(" + i + "," + j + ")";
			m_Grid[i, j] = kachel;
			kachel.transform.position = new Vector3 (i, j, 0);
			kachel.transform.parent = this.transform;
		} 
		Camera.main.transform.position = new Vector3 (m_iSize/2, m_iSize/2, -100);
		Camera.main.orthographicSize = m_iSize;
		
		transform.position = new Vector3 (0.5f, 0.5f, 0);
		
		
		for (int i = 0; i < m_iSize; i++)
		for (int j = 0; j < m_iSize; j++) 
		{
			float farben = Random.value;
			if (farben <= 0.5)
				SetAlive (i, j, true);//m_Grid [i, j].GetComponent<Renderer>().material.color = Color.blue;
			else
				SetAlive (i, j, false);//m_Grid [i, j].GetComponent<Renderer>().material.color = Color.white;
		}
		print ("Anzahl Nachbarn: " + GetAliveNeighbours (1, 1));
		Debug.Log (GetAliveNeighbours (9, 9));


		//KillAll ();
	}
	
	
	int GetAliveNeighbours (int _iColumn, int _iRow)
	 {
		int AliveNeighboursCounter = 0;
		for (int i = 0; i < 3; i++) 
		{
			int iTmp = i + _iColumn - 1;
			for (int j = 0; j < 3; j++)
			{
				int jTmp = j + _iRow - 1;
				if (!(iTmp == _iColumn && jTmp == _iRow) && !(iTmp < 0) && !(jTmp < 0) && (iTmp < m_iSize) && (jTmp < m_iSize))
					if (GetAlive(iTmp, jTmp))//m_Grid [iTmp, jTmp].GetComponent<Renderer>().material.color == Color.blue)
						AliveNeighboursCounter ++;
			}
			
		}
		
		return AliveNeighboursCounter;
	}


	
	void Update (){
		if (Input.GetKeyDown (KeyCode.K))
			KillAll ();

		if (Input.GetMouseButtonDown (0)) 
		{
			Vector3 MouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			int iIndexX = (int)MouseWorldPos.x;
			int iIndexY = (int)MouseWorldPos.y;
			if (iIndexX <= m_iSize && iIndexX > 0 && iIndexY <= m_iSize && iIndexY > 0)
			Toggle (iIndexX, iIndexY);
		}
		//	print (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		//create array to store num alive neighbours before changing anything
		int [,] m_iAliveNeighbours = new int[m_iSize, m_iSize];
		//store num alive neighbours
		for (int i = 0; i < m_iSize; i++)
			for (int j = 0; j < m_iSize; j++) {
				m_iAliveNeighbours[i, j] = GetAliveNeighbours (i, j);
			}


		if (Input.GetKeyDown (KeyCode.Space) == false)
			return;
		//Change stuff
		for (int iColumn = 0; iColumn < m_iSize; iColumn++)
		for (int iRow = 0; iRow < m_iSize; iRow++){
			int iNumAlive = m_iAliveNeighbours [iColumn, iRow];
			//Alive
			if (GetAlive (iColumn, iRow)){
				if (iNumAlive < 2 || iNumAlive > 3)
					SetAlive(iColumn, iRow, false);//m_Grid[iColumn, iRow].GetComponent<Renderer>().material.color = Color.white;
			}
			//Dead
			else
				if (iNumAlive == 3)
			SetAlive(iColumn, iRow, true);//m_Grid[iColumn, iRow].GetComponent<Renderer>().material.color = Color.blue;
			
			
			
		}
	}
}