// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileRef.Pagination.IOS
{
	[Register ("ArticleTableController")]
	partial class ArticleTableController
	{
		[Outlet]
		UIKit.UITableView lstArticles { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lstArticles != null) {
				lstArticles.Dispose ();
				lstArticles = null;
			}
		}
	}
}
