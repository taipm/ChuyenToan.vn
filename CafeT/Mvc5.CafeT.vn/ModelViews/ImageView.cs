using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ImageView
    {
        public ImageModel Model { set; get; }

        public ImageView(ImageModel model)
        {
            Model = model;
        }
    }
}