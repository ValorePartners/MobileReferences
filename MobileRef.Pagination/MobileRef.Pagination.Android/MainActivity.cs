using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using MobileRef.Pagination.Shared;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using System.ComponentModel;
using PullListView = PullToRefresharp.Android.Widget.ListView;

namespace MobileRef.Pagination.Android
{
	[Activity (Label = "MobileRef.Pagination.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class ArticleTable : Activity,IHandlers
	{
		public EditText txtSearch{ get; private set; }
		public Button btnSearch{ get; private set; }
		public PullListView lstArticles{ get; private set; }
		public ArticleCustomAdapter Adapter{ get; set;}
		public BindableHUD Progress{ get; set;}
		private BindingManager<ArticleTable,MedicalArticleViewModel> bind;

//		private PullListView pullList{
//			get{ return (PullListView)lstArticles; }
//		}

		public MedicalArticleViewModel VM {
			get {
				return AppData.ArticlesVM;
			}
			set {
				AppData.ArticlesVM = value;
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.ArticleTable);

			this.CreateControls<ArticleTable>();
			Progress = new BindableHUD (this);
			bind = new BindingManager<ArticleTable, MedicalArticleViewModel> ();
			bind.BindProperty (() => txtSearch.Text, () => VM.SearchTerm);
			bind.BindProperty (() => Progress.Message, () => VM.LoadMessage);
			bind.BindProperty (() => Progress.Visible, () => VM.IsLoading);
			bind.BindCommand (() => btnSearch, ()=>VM.PerformSearch);

			Adapter = new ArticleCustomAdapter (VM.Articles.ToArray (), this.LayoutInflater);

			lstArticles.Adapter = Adapter;
			bind.BindProperty (() => Adapter.Items, () => VM.Articles);

			lstArticles.Scroll+=(e,a)=>{
				if(a.TotalItemCount>a.VisibleItemCount){
					int lastInScreen = a.FirstVisibleItem + a.VisibleItemCount;    
					if((lastInScreen == a.TotalItemCount) && !Adapter.IsPaginating){ 
						Adapter.IsPaginating=true;
						VM.Paginate.Execute(null);
						lstArticles.SetSelection(-1);
					}
				}
			};

			lstArticles.RefreshActivated+=(e,a)=>{
				VM.Refresh.Execute(null);
			};
		}

		protected override void OnResume ()
		{
			bind.RegisterBindingEvents (this, VM);
			base.OnResume ();
		}
		protected override void OnPause ()
		{
			bind.UnRegisterBindingEvents (this);
			base.OnPause ();
		}

		public void ViewEventHandler (object e, EventArgs args)
		{
			if (args is PropertyChangedEventArgs) {
				var propName = ((PropertyChangedEventArgs)args).PropertyName;
				switch (propName) {
				case "Articles":
					lstArticles.OnRefreshCompleted();
					break;
				}
			}
		}

		public void ControlsHandler (object sender, EventArgs args)
		{
	
		}

	}

	public class ArticleCustomAdapter:BaseAdapter<MedicalArticle>,IBaseAdapter{

		public MedicalArticle[] Items { get; set; }
		private LayoutInflater inflator;
		public bool IsPaginating{ get; set; }
		public ArticleCustomAdapter (MedicalArticle[] items, LayoutInflater inflator)
		{
			this.Items = items;
			this.inflator = inflator;
		}
		public void UpdateCollection (System.Collections.ICollection collection)
		{
			var list = new List<MedicalArticle> ();
			foreach (var obj in collection) {
				list.Add ((MedicalArticle)obj);
			}
			Items = list.ToArray ();
			this.NotifyDataSetChanged ();
			IsPaginating = false;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = Items [position];
			var view = convertView;
			if (view == null) {
				view = inflator.Inflate (Resource.Layout.ArticleCell, null);
			} 
			var txtDescription = view.FindViewById<TextView> (Resource.Id.txtDescription);
			txtDescription.Text = item.Description;
			return view;
		}

		public override int Count {
			get {
				return Items.Length;
			}
		}


		public override MedicalArticle this [int index] {
			get {
				return Items [index];
			}
		}



	}
}


