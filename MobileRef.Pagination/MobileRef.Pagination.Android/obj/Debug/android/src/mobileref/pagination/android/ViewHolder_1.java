package mobileref.pagination.android;


public abstract class ViewHolder_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MobileRef.Pagination.Android.ViewHolder`1, MobileRef.Pagination.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ViewHolder_1.class, __md_methods);
	}


	public ViewHolder_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ViewHolder_1.class)
			mono.android.TypeManager.Activate ("MobileRef.Pagination.Android.ViewHolder`1, MobileRef.Pagination.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
