namespace SimpleSQL.Demos
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SimpleSQL;
    using System.IO;

    ///  <summary>
    /// This script loads blobs from a database table and converts them to images,
    /// displaying them as UI elements in a layout group.
    ///  </summary>
    public class Blobs : MonoBehaviour
    {
        // reference to the database manager in the scene
        public SimpleSQLManager dbManager;

        // reference to the input field that stores the path to a new image to add
        public UnityEngine.UI.InputField imagePath;

        // reference to the UI group to add images to
        public UnityEngine.UI.LayoutGroup imageGroup;

        // reference to the UI element that will show images
        public GameObject imagePrefab;

        // Initialize
        void Start()
        {
            // Load the images
            LoadImages();
        }

        ///  <summary>
        /// Loads blob data from database and converts to images
        ///  </summary>
        private void LoadImages()
        {
            // clear out previous image UI elements
            foreach (var t in imageGroup.GetComponentsInChildren<Transform>())
            {
                if (t != imageGroup.transform)
                {
                    GameObject.Destroy(t.gameObject);
                }
            }

            // load data from database
            var sql = "SELECT ID, ImageData FROM Images";
            var results = dbManager.Query<Images>(sql);

            // display results
            foreach (var result in results)
            {
                // set up a new texture element. The size is irrelevant since it will be overwritten
                var texture = new Texture2D(2, 2);
                // load image data from database blob (byte array)
                texture.LoadImage(result.ImageData);

                // instantiate the ui element
                var uiImageObj = GameObject.Instantiate(imagePrefab);
                uiImageObj.transform.SetParent(imageGroup.transform);

                // get the image component of the ui element and set its size and Sprite
                // based on the texture2D data loaded above
                var uiImage = uiImageObj.GetComponent<UnityEngine.UI.Image>();
                uiImageObj.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
                uiImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // get the ui element script and set up its properties and delegates
                var blobImage = uiImageObj.GetComponent<BlobImage>();
                blobImage.Set(result, DeleteImage);
            }

        }

        ///  <summary>
        /// Adds and image to the database as a blob
        ///  </summary>
        public void AddImage()
        {
            var path = imagePath.text.Trim();

            // check that the path exists
            if (!File.Exists(path))
            {
                Debug.LogError("Path does not exist! " + imagePath.text);
                return;
            }

            var id = 1;
            // read the bytes of the file from the path
            var imageData = File.ReadAllBytes(path);

            // get the next ID from the database
            var sql = "SELECT MAX(ID) + 1 ID FROM Images";
            bool recordExists;
            var result = dbManager.QueryFirstRecord<Images>(out recordExists, sql);
            if (recordExists)
            {
                id = result.ID;
            }

            // insert the image as a byte array
            sql = "INSERT INTO Images (ID, ImageData) VALUES (?, ?)";
            dbManager.Execute(sql, id, imageData);

            // reload images
            LoadImages();
        }

        ///  <summary>
        /// Deletes and image from the database. Called from the UI element
        ///  </summary>
        public void DeleteImage(int ID)
        {
            // delete the image based on its ID
            var sql = "DELETE FROM Images WHERE ID = ?";
            dbManager.Execute(sql, ID);

            // reload images
            LoadImages();
        }
    }
}