using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Clicker;

public class scene1_updater : MonoBehaviour 
{
    private List<ClickerHit> Clickers = new List<ClickerHit>();
	private const double HeldTimeDefault = 0.35;

    private const int ClickerLifeMax = 55;
    private int _effectCount = 0;

    private const int SquishScore = 200;
	private int _score;

	public Texture2D HandOpen;
	public Texture2D HandClosed;
    public List<Texture2D> ClickImages;

	private Texture2D _currentHand;
    private Vector3 _handSize;

    public AudioClip Crunch1;
	public AudioClip Crushing1;
	public AudioClip Crushing2;
	public AudioClip Crushing3;

    public Canvas guiCanvasObject;

	private double _heldTime;
	private bool _menuOpen;
	private float _handXOffset = -1.0f;
	private float _handYOffset = -1.0f;
	private int _numHits;

	// Use this for initialization
	void Start () {
		print ("HELLOOOOOOO VIET-WORLD!");
		_menuOpen = false;
		_heldTime = 0;
		_score = 0;
		_numHits = 0;

		Cursor.visible = false;

        Clickers = new List<ClickerHit>();
        _effectCount = ClickImages.Count();
    }

    // Update is called once per frame
    void Update () {
        //Cannot get to menu in web player
        if (!Application.isWebPlayer)
        {
            //Menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _menuOpen = !_menuOpen;
                if (_menuOpen)
                {
                    Time.timeScale = 0;
                    Cursor.visible = true;
                }
                else
                {
                    Time.timeScale = 1;
                    Cursor.visible = false;
                }
            }
        }		

		if (!_menuOpen) 
		{
			//on first frame of click, set default time
			if (Input.GetMouseButtonDown(0))
			{
                //add hit animation
                Clickers.Add(new ClickerHit
                {
                    location = new Vector2(Input.mousePosition.x, Input.mousePosition.y),
                    TimeAlive = 0
                });

				_heldTime = HeldTimeDefault;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				RaycastHit hitData;
				if (Physics.Raycast(ray, out hitData))
				{
					GameObject hitObject = hitData.collider.gameObject;

					SquishyTarget squishTarget = hitObject.GetComponent<SquishyTarget>();
					if (squishTarget != null && !squishTarget.IsDead)
					{
						squishTarget.SquishHead();
						
						_numHits++;

						ScorePoints();
					}
					else
						_numHits = 0;

				}
				else
					_numHits = 0;
			}

			if (Input.GetMouseButton(0))
				_currentHand = HandClosed;
			else if (_heldTime <= 0)
				_currentHand = HandOpen; //open if held time is finished

			//decrement held timer
			if (_heldTime > 0)
				_heldTime -= Time.deltaTime;

		}
	}

	void ScorePoints()
	{
		print ("Woohoo, points!");

		_score += SquishScore * _numHits;

		AudioClip clip;

		switch (_numHits)
		{
		case 1:
			clip = Crunch1;
			break;
		case 2:
			clip = Crushing1;
			break;
		case 3:
			clip = Crushing2;
			break;
		default:
			clip = Crushing3;
			break;
		}

        //if we get to last hit, repeat hit 2 and skip hit 1
		if (_numHits == 4)
			_numHits = 1;

		GetComponent<AudioSource>().PlayOneShot (clip);
	}

    private void RenderClickers()
    {
        foreach (var hit in Clickers)
        {
            //find effect number. Get percent progress and find closest effect count
            int effectIndex = (int)Math.Round((double)((double)_effectCount * ((double)hit.TimeAlive / (double)ClickerLifeMax)));
            effectIndex = effectIndex > _effectCount - 1 ? _effectCount - 1 : effectIndex;
            //get image
            var clickImage = ClickImages[effectIndex];

            var clickSize = new Vector3(clickImage.width, clickImage.height);

            //draw click effect
            var x = hit.location.x - clickSize.x / 2;
            var y = hit.location.y + clickSize.y / 2;
            GUI.Label(new Rect(x, Screen.height - y, clickSize.x, clickSize.y), clickImage);

            hit.TimeAlive++;
        }

        //remove dead hits
        Clickers.RemoveAll(h => h.TimeAlive > ClickerLifeMax);
    }

    private void RenderHand()
    {
        if (_handXOffset == -1.0f)
        {
            _handSize = new Vector3(_currentHand.width, _currentHand.height);
            _handXOffset = _handSize.x * -0.065f;
            _handYOffset = _handSize.y * 0.35f;
        }

        var handX = Input.mousePosition.x + _handXOffset;
        var handY = Input.mousePosition.y + _handYOffset;
        GUI.Label(new Rect(handX + _handXOffset, Screen.height - handY, _handSize.x, _handSize.y), _currentHand);
    }

	void OnGUI()
	{
		if (_menuOpen)
		{
			GUI.Box(new Rect(10,10,100,90), "Loader Menu");

			if(GUI.Button(new Rect(20,40,80,20), "Quit"))
			{
				Application.Quit();
			}
		}

        RenderHand();

        RenderClickers();

        GUI.Label (new Rect (Screen.width - 200, 10, 200, 50), "Score: " + _score.ToString());
	}
}
