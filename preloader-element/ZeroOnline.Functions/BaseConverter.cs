using System;
using System.Windows.Markup;

namespace ZeroOnline.Functions;

public abstract class BaseConverter : MarkupExtension
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
