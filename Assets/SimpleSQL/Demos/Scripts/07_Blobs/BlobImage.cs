namespace SimpleSQL.Demos
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    ///  <summary>
    /// This is a prefab UI element that displays an image
    ///  </summary>
    public class BlobImage : MonoBehaviour
    {
        // ID of the image
        private int id;

        // delete delegate
        private Action<int> deleteAction;

        ///  <summary>
        /// Called when the UI element is set up
        ///  </summary>
        public void Set(Images data, Action<int> deleteAction)
        {
            this.id = data.ID;
            this.deleteAction = deleteAction;
        }

        ///  <summary>
        /// Called when delete button is pressed on UI element
        ///  </summary>
        public void Delete()
        {
            if (deleteAction != null)
            {
                // Call the delete delegate
                deleteAction(id);
            }
        }
    }
}