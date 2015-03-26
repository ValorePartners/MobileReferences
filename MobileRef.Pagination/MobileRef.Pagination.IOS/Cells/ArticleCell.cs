
using System;

using Foundation;
using UIKit;
using MobileRef.Pagination.Shared;

namespace MobileRef.Pagination.IOS
{
	public partial class ArticleCell : UITableViewCell,IViewCell<MedicalArticle>
	{
		public static readonly UINib Nib = UINib.FromName ("ArticleCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ArticleCell");

		public ArticleCell (IntPtr handle) : base (handle)
		{
		}

		#region IViewCell implementation
		public void Bind (MedicalArticle item)
		{
			this.lblTitle.Text = item.Title;
			this.lblDescription.Text = item.Description;
		}
		#endregion

		public static ArticleCell Create ()
		{
			return (ArticleCell)Nib.Instantiate (null, null) [0];
		}
	}
}

