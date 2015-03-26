using System;
using System.Collections.Generic;

namespace MobileRef.Pagination.Shared
{
	public class Entry
	{
		public string arxId { get; set; }
		public string blurbTitle { get; set; }
		public string blurbText { get; set; }
		public string imageUrl { get; set; }
		public string articleUrl { get; set; }
		public string articleFullUrl { get; set; }
		public string type { get; set; }
		public string doi { get; set; }
		public string isOpenAccess { get; set; }
		public string isFree { get; set; }
		public string isHighlyAccessed { get; set; }
		public string bibliograhyTitle { get; set; }
		public string authorNames { get; set; }
		public string longCitation { get; set; }
		public string status { get; set; }
		public string abstractPath { get; set; }

	}

	public class RootObject
	{
		public List<Entry> entries { get; set; }
	}

	public class MedicalArticle{

		public string Title{ get; set; }
		public string Description{ get; set; }
		public string Author{ get; set; }
		public string DOI{ get; set; }
		public string ImageURL{ get; set; }
		public string ArticleURL{ get; set; }

		public MedicalArticle ()
		{
			
		}
		public MedicalArticle (Entry entry)
		{
			this.Title = entry.blurbTitle;
			this.Description = entry.bibliograhyTitle;
			this.Author = entry.authorNames;
			this.DOI = entry.doi;
			this.ImageURL = entry.imageUrl;
			this.ArticleURL = entry.articleUrl;
		}
	}
}

