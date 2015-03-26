

namespace  MobileRef.Pagination.Shared
{
	public class AppData
	{
		private static LocalStorageService storage;

		private static MedicalArticleViewModel articlesVM;

		public static bool IsConnected{ get; set; }

		public static LocalStorageService Storage {
			get {
				if (storage == null)
					storage = new LocalStorageService ();
				return storage;
			}
		}

		//example of get global view model from state management
		public static MedicalArticleViewModel ArticlesVM {
			get {
				if (articlesVM == null) {
					articlesVM = Storage.GetIsolatedStorage<MedicalArticleViewModel> ("MedicalArticleViewModel");
					if (articlesVM == null)
						articlesVM = new MedicalArticleViewModel ();
				}
				return articlesVM;
			}
			set {
				articlesVM = value;
			}
		}


		public static async void SaveDatabaseVM ()
		{
			//example of saving global view model to state
			await Storage.SaveIsolatedStorageAsync<MedicalArticleViewModel> ("MedicalArticleViewModel", articlesVM);
		}

		public static void ClearStateManagement ()
		{
			//example of clearing data from state
			Storage.DeleteIsolatedStorage ("MedicalArticleViewModel");
		}


	}


}
