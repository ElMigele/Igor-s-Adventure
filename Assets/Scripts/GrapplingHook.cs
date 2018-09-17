/**************************************************************************************************/
/** 	© 2017 NULLcode Studio. License: https://creativecommons.org/publicdomain/zero/1.0/deed.ru
/** 	Разработано в рамках проекта: http://null-code.ru/
/**                       ******   Внимание! Проекту нужна Ваша помощь!   ******
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/**************************************************************************************************/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]

public class GrapplingHook : MonoBehaviour {

	[SerializeField] private float swingForce = 50; // сила импульса для раскачивания
	[SerializeField] private LineRenderer ropeLine; // линия веревки
	[SerializeField] private LineRenderer targetLine; // линия указатель
	[SerializeField] private float minDistance = 2; // минимальная длинна веревки
	[SerializeField] private float step = .2f; // шаг сокращения длинны
	[SerializeField] [Range(0.1f, 1f)] private float offset = .5f; // насколько объект подпрыгивает, когда цепляет веревку
	[SerializeField] private LayerMask layerMask; // фильтр объектов, за которые можно цепляться
	private DistanceJoint2D joint;
	private Vector2 mousePosition;
	private RaycastHit2D hit;
	private float distance, last;
	public static bool isActive { get; private set; }
	private Rigidbody2D body;
	private bool swing;

	void Awake() 
	{
		body = GetComponent<Rigidbody2D>();
		joint = GetComponent<DistanceJoint2D>();
		joint.enabled = false;
		joint.enableCollision = true;
		joint.maxDistanceOnly = true;
		joint.autoConfigureDistance = false;
		joint.autoConfigureConnectedAnchor = false;
		joint.distance = minDistance;
		isActive = false;
	}

	void TargetLine()
	{
		RaycastHit2D target = Physics2D.Linecast(transform.position, mousePosition, layerMask);

		if(target.transform != null)
		{
			targetLine.positionCount = 2;
			targetLine.SetPosition(0, transform.position);
			targetLine.SetPosition(1, target.point);
		}
		else
		{
			targetLine.positionCount = 0;
		}
	}

	void RopeLine()
	{
		if(joint.enabled)
		{
			ropeLine.positionCount = 2;
			ropeLine.SetPosition(0, transform.position);
			ropeLine.SetPosition(1, joint.connectedAnchor);
		}
		else
		{
			ropeLine.positionCount = 0;
		}
	}

	bool CanUse(Vector3 position)
	{
		hit = Physics2D.Linecast(transform.position, position, layerMask);

		if(hit.transform != null)
		{
			distance = Vector2.Distance(transform.position, hit.point);
			return true;
		}

		return false;
	}

	void LateUpdate()
	{
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		TargetLine();
		RopeLine();
		MainControl();
		SwingControl();
	}

	Vector3 GetDirection() // перпендикуляр к веревке
	{
		return Vector3.Cross(hit.point - body.position, Vector3.forward).normalized;
	}

	void SetActive()
	{
		joint.connectedAnchor = hit.point;
		joint.distance = distance - offset;
		joint.enabled = true;
		isActive = true;
	}

	void MainControl()
	{
		if(Input.GetMouseButtonDown(0) && CanUse(mousePosition))
		{
			SetActive();
			swing = false;
		}
		else if(Input.GetMouseButtonDown(1) && CanUse(mousePosition))
		{
			SetActive();
			swing = true;
			last = 0;
		}

		if(Input.GetMouseButton(0) && joint.enabled)
		{
			if(joint.distance > minDistance)
			{
				joint.distance -= step;
			}
		}
		else if(Input.GetMouseButtonUp(0) && joint.enabled)
		{
			isActive = false;
			joint.enabled = false;
		}
	}

	void SwingControl()
	{
		if(!swing) return;

		float h = Input.GetAxis("Horizontal");
		if(h > 0) h = 1; else if(h < 0) h = -1;

		if(last != h && h != 0)
		{
			body.AddForce(GetDirection() * h * swingForce, ForceMode2D.Impulse);
		}

		if(h != 0) last = h;

		if(Input.GetKeyDown(KeyCode.Space)) // отцепиться
		{
			swing = false;
			isActive = false;
			joint.enabled = false;
		}
	}
}
