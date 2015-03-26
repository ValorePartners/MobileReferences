using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MobileRef.Pagination.Shared
{
	public class MedicalArticleViewModel:VMBase
	{
		private ObservableCollection<MedicalArticle> articles;
		private string searchTerm;
		private RestService http;

		public int CurrentIndex{ get; set; }

		public ICommand PerformSearch{ get; set; }

		public ICommand Paginate{ get; set; }

		public ICommand Refresh{ get; set; }

		public string SearchTerm {
			get {
				return searchTerm;
			}
			set {
				searchTerm = value;
				Notify ("SearchTerm");
			}
		}

		public ObservableCollection<MedicalArticle> Articles {
			get {
				return articles;
			}
			set {
				articles = value;
				Notify ("Articles");
			}
		}

		public MedicalArticleViewModel ()
		{
			CurrentIndex = 1;
			http = new RestService ();
			Articles = new ObservableCollection<MedicalArticle> ();
			PerformSearch = new RelayCommand (async() => {
				CurrentIndex = 1;
				articles.Clear ();
				LoadMessage = "Searching term...";
				IsLoading = true;
				var data = await GetArticles (SearchTerm, CurrentIndex);
				Articles = data.ToObservable ();
				IsLoading = false;
				CurrentIndex++;
			});
			Paginate = new RelayCommand (async() => {
				if (!string.IsNullOrEmpty (searchTerm)) {
					LoadMessage = "Searching term...";
					IsLoading = true;
					var data = await GetArticles (SearchTerm, CurrentIndex);
					var temp = articles.ToList ();
					temp.AddRange (data);
					Articles = temp.ToObservable ();
					IsLoading = false;
					CurrentIndex++;
				}
			});
			Refresh = new RelayCommand (async() => {
				var total = CurrentIndex * 10;
				articles.Clear ();
				var data = await GetArticles (SearchTerm, 1, total);
				Articles = data.ToObservable ();
			});
		}

		private async Task<List<MedicalArticle>> GetArticles (string term, int index, int total = 10)
		{
			var list = new List<MedicalArticle> ();
			var url = "http://www.biomedcentral.com/search/results?terms={0}&itemsPerPage={1}&page={2}&format=json";
			var formattedUrl = string.Format (url, term, total, index);
			var result = await http.GetAsync<RootObject> (formattedUrl);
			foreach (var obj in result.entries) {
				list.Add (new MedicalArticle (obj));
			}
			return list;
		}

	}
}

