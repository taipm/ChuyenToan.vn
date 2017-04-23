using CafeT.BusinessObjects;

namespace Mvc5.CafeT.vn.Models
{
    public class ProductModel : BaseObject
    {
        public string Name { set;get; }
        public string Description { set; get; }
        public double Price { set; get; }
 
        public ImageObject[] Images{ set; get; }

        public ProductModel():base()
        { }

        public void UploadImages()
        {

        }
    }
}