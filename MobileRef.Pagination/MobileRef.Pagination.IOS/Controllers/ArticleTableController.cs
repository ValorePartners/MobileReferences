using System;
using Foundation;
using UIKit;
using MobileRef.Pagination.Shared;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace MobileRef.Pagination.IOS
{
	public partial class ArticleTableController : UIViewController,IHandlers
	{


		private UIRefreshControl refreshControl;
		public BindableProgress Progress{ get; set; }

		private BindingManager<ArticleTableController,MedicalArticleViewModel> bind;

		public ArticleCustomSource Source{ get; set; }

		public MedicalArticleViewModel VM {
			get {
				return AppData.ArticlesVM;
			}
			set {
				AppData.ArticlesVM = value;
			}
		}


		public ArticleTableController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Progress = new BindableProgress (this.View);
		
			bind = new BindingManager<ArticleTableController, MedicalArticleViewModel> ();
			Source = new ArticleCustomSource (VM.Articles.ToArray ()){ TableView = lstArticles };
			lstArticles.Source = Source;

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (UIBarButtonSystemItem.Action, (sender, args) => {
					this.ShowMessage ("Enter search criteria", string.Empty, "Search", "Term", UIKeyboardType.Default, (str) => {
						if (!string.IsNullOrEmpty (str)) {
							VM.SearchTerm = str;
							VM.PerformSearch.Execute (null);
						}
					});
				})
				, true);

			bind.BindProperty (() => Source.Items, () => VM.Articles);
			bind.BindProperty (() => Progress.IsVisible, () => VM.IsLoading);
			bind.BindProperty (() => Progress.LoadingMessage, () => VM.LoadMessage);
			bind.BindCommand ("Source", "PaginationEvent", "Paginate");

			refreshControl = new UIRefreshControl ();
			refreshControl.AttributedTitle = new NSAttributedString ("Loading data...");

			lstArticles.AddSubview (refreshControl);


			refreshControl.ValueChanged +=(e,a)=>{
				VM.Refresh.Execute(null);
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			bind.RegisterBindingEvents (this, VM);
			base.ViewWillAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			bind.UnRegisterBindingEvents (this);
			base.ViewWillDisappear (animated);
		}

		public void ViewEventHandler (object e, EventArgs args)
		{
			if (args is PropertyChangedEventArgs) {
				var propName = ((PropertyChangedEventArgs)args).PropertyName;
				switch (propName) {
				case "Articles":
					refreshControl.EndRefreshing();
					break;
				}
			}
		}

		public void ControlsHandler (object sender, EventArgs args)
		{
			
		}

	}

	public class ArticleCustomSource:UITableViewSource,IBaseSource
	{
		private string cellId = "articleCellId";

		public event EventHandler RowSelectedEvent;
		public event EventHandler PaginationEvent;

		public UITableView TableView { get; set; }

		public object SelectedItem { get; set; }

		public nint Tag { get; set; }

		public MedicalArticle[] Items { get; set; }

		private bool isPagingating;

		public ArticleCustomSource (MedicalArticle[] items)
		{
			this.Items = items;
		}

		public void UpdateCollection (System.Collections.ICollection collection)
		{
			var list = new List<MedicalArticle> ();
			foreach (var obj in collection) {
				list.Add ((MedicalArticle)obj);
			}
			Items = list.ToArray ();
			TableView.ReloadData ();
			if (isPagingating)
				isPagingating = false;
		}


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (TableView == null)
				TableView = tableView;

			var cell = tableView.DequeueReusableCell (cellId);
			if (cell == null) {
				cell = ArticleCell.Create ();
			}
			((ArticleCell)cell).Bind (Items [indexPath.Row]);
			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Items.Length;
		}

		public override void Scrolled (UIScrollView scrollView)
		{
			var totalRenderedHeight = TableView.RowHeight * Items.Length;
			var maxHeight = totalRenderedHeight + (.75 * TableView.RowHeight);
			var currentHeight = TableView.Frame.Size.Height;
			var offSet = scrollView.ContentOffset.Y;
			if (offSet > 0) {
				var distanceFromBottom = currentHeight + offSet;
				if ((distanceFromBottom > maxHeight) && (totalRenderedHeight > currentHeight)) {
					if (PaginationEvent != null && !isPagingating) {
						isPagingating = true;
						PaginationEvent (this, null);
					}
				}
			}
		}
	}
}
