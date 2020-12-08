using Microsoft.AspNetCore.Components;
using NFTIntegration.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFTIntegration.Classes
{
    public class TabsBase : ComponentBase
    {
        public int Selected { get; set; }
        public bool IsTabAdded { get; set; }

        public RenderFragment GetRenderFragment(Type type)
        {
            RenderFragment renderFragment = renderTreeBuilder =>
            {
                renderTreeBuilder.OpenComponent(0, type);
                renderTreeBuilder.CloseComponent();
            };

            return renderFragment;
        }

        public List<ComponentBase> PageComponents = new List<ComponentBase> { new About() };

        public List<Type> ComponentTypes => PageComponents.Select(t => t.GetType()).ToList();

        public void AddTabs(string tabName)
        {
            var isExists = ComponentTypes.Where(x => x.Name.Equals(tabName)).Count() > 0;

            if (!isExists)
            {
                Type type = null;

                switch (tabName.ToUpper())
                {
                    case "ABOUT":
                        var about = new About();
                        PageComponents.Add(about);
                        type = about.GetType();
                        break;
                    case "NMAP":
                        var nmap = new Nmap();
                        PageComponents.Add(nmap);
                        type = nmap.GetType();
                        break;
                    case "ZAP":
                        var zap = new Zap();
                        PageComponents.Add(zap);
                        type = zap.GetType();
                        break;
                }

                GetRenderFragment(type);
            }
        }
    }
}
