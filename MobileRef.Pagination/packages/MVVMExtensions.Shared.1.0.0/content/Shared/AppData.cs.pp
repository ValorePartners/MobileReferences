

namespace  $rootnamespace$
{
	public class AppData
	{
		private static LocalStorageService storage;

		//example of view model as global entity
		//private static DatabaseViewModel databaseVM;

		/// <summary>
		/// Use this global variable to save connectivity state
		/// </summary>
		/// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
		public static bool IsConnected{ get; set; }

		public static LocalStorageService Storage {
			get {
				if (storage == null)
					storage = new LocalStorageService ();
				return storage;
			}
		}

		//example of get global view model from state management
		//		public static DatabaseViewModel DatabaseVM {
		//			get {
		//				if (databaseVM == null) {
		//					databaseVM = Storage.GetIsolatedStorage<DatabaseViewModel> ("DatabaseViewModel");
		//					if (databaseVM == null)
		//						databaseVM = new DatabaseViewModel ();
		//				}
		//				return databaseVM;
		//			}
		//			set {
		//				databaseVM = value;
		//			}
		//		}


		public static async void SaveDatabaseVM ()
		{
			//example of saving global view model to state
			//await Storage.SaveIsolatedStorageAsync<DatabaseViewModel> ("DatabaseViewModel", databaseVM);
		}

		public static void ClearStateManagement ()
		{
			//example of clearing data from state
			//Storage.DeleteIsolatedStorage ("DatabaseViewModel");
		}


	}


}
