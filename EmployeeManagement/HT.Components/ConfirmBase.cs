using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Components
{
    public class ConfirmBase : ComponentBase
    {
        public bool ShowConfirmation { get; set; }

        public void Show()
        {
            ShowConfirmation = true;
        }

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }
    }
}
