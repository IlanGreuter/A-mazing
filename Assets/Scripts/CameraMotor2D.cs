using UnityEngine;

namespace IlanGreuter
{
    public class CameraMotor2D : MonoBehaviour
    {
        [SerializeField, Tooltip("Automatically grabs Camera.main if unset")]
        internal Camera cam;
        internal Transform t;

        //Zoom
        [Header("Zooming")]
        [SerializeField, Min(0.01f)] private float targetZoom = 8f;
        [SerializeField, Min(0.01f)] private float minZoom = 1f, maxZoom = 12f;
        [SerializeField, Tooltip("Interval to which targetZoom snaps")] private float zoomStep = 1f;
        [SerializeField, Min(0)] private float zoomDuration = 0.5f;
        private float zoomChange;

        //Move
        [Header("Scrolling")]
        [SerializeField] private float scrollSpeed = 12f;
        [SerializeField, Tooltip("The base zoom at which the scroll multiplier is equal to one")]
        private float scrollBaseZoom = 8f;
        [SerializeField, Tooltip("The modifier to scroll speed based on current zoom level")]
        private float scrollZoomModifier = 0.2f;
        private Vector2 moveInput;
        
        //Rotate
        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 30f;
        private float rotationInput;

        ///TODO extras:
        ///Rework scrolling zoomMultiplier 
        ///Lock rotation
        ///Edge panning (increase speed when mouse is further out?)
        ///Drag (middle mouse down)
        ///Max  panning distance if local move?
        ///Zoom in towards mouse
        ///Look towards mouse?
        ///Input system compatability

        /// <summary> The current position of the camera </summary>
        public Vector3 Position
        {
            get { return t.position; }
            set { t.position = value; }
        }

        /// <summary> The current Rotation of the camera </summary>
        public float Rotation
        {
            get { return t.eulerAngles.z; }
            set { t.eulerAngles = value * Vector3.forward; }
        }

        /// <summary> Translates the camera by this vector  </summary>
        public void Translate(Vector3 movement) => t.Translate(movement, Space.World);

        /// <summary> Rotate the camera by this many degrees </summary>
        public void Rotate(float degrees) => t.Rotate(degrees * Vector3.forward, Space.World);

        internal virtual void Awake()
        {
            if (cam == null)
                cam = Camera.main;
            t = cam.transform;
        }

        internal virtual void Update()
        {
            HandleInput();
            UpdateZoom();
            UpdateRotation();
            UpdatePosition();
        }

        #region Zoom Methods
        /// <summary> The current zoom of the camera. </summary>
        public virtual float Zoom
        {
            get { return cam.orthographicSize; }
            set { cam.orthographicSize = Mathf.Clamp(value, minZoom, maxZoom); }
        }

        private void UpdateZoom()
        {
            //Increase the target zoom
            ChangeZoomTarget(-Input.mouseScrollDelta.y * zoomStep);

            float currentZoom = Zoom;
            if (currentZoom == targetZoom)
                return;

            //Adjust the zoom
            Zoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomChange, zoomDuration * 0.3f);

            //Round the zoom if its close to target
            if (Mathf.Abs(currentZoom - targetZoom) < 0.01f)
                Zoom = targetZoom;
        }

        /// <summary> Set the zoom target to this. </summary>
        public void SetZoomTarget(float zoom) => targetZoom = zoom;
        /// <summary> Add the specified amount to the zoom target. 
        /// Positive values will zoom in, negative values will zoom out. </summary>
        public void ChangeZoomTarget(float zoomChange) =>
            targetZoom = Mathf.Clamp(targetZoom + zoomChange, minZoom, maxZoom);
        #endregion Zoom Methods

        private void UpdateRotation()
        {
            Rotate(rotationInput * Time.deltaTime);
        }

        private void UpdatePosition()
        {
            float zoomBonus = 1f + Mathf.Clamp(scrollZoomModifier * (Zoom - scrollBaseZoom), -1f, 1f);
            Translate(moveInput * (zoomBonus * Time.deltaTime));
        }

        private void HandleInput()
        {
            ChangeZoomTarget(-Input.mouseScrollDelta.y * zoomStep);
            rotationInput = Input.GetAxis("Rotate") * rotationSpeed;
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * scrollSpeed;

            //Vector2 mousePos = Input.mousePosition;
            //bool panningKeyDown = Input.GetMouseButtonDown(2);
        }
    }
}