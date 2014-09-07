/* pure js no jQuery */
function boxes_checkall(form,skip)
	{
		for (i=0,x=form.elements.length;i<x;i++)
		{
			if ( form.elements[i].type == "checkbox" )
			{
				if (skip == 1)
				{
					form.elements[i].checked = true;
				}
				else
				{
					if (form.elements[i].checked == true)
					{
						form.elements[i].checked = false;
					}
					else
					{
						form.elements[i].checked = true;
					}
				}
			}
		}
	}