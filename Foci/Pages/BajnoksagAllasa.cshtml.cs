using Foci.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foci.Pages
{
    public class BajnoksagAllasaModel : PageModel
    {
        private readonly FociDbContext _context;

        public BajnoksagAllasaModel(FociDbContext context)
        {
            _context = context;
        }

        public List<Meccs> meccsek;
        public List<CsapatEredmenyei> csapatEredmenyei;

        public void OnGet()
        {
            meccsek = _context.Meccsek.ToList();
            csapatEredmenyei = new List<CsapatEredmenyei>();

            foreach (var csapat in meccsek.Select(x => x.HazaiCsapat).Distinct())
            {
                CsapatEredmenyei ujCsapat = new CsapatEredmenyei();
                ujCsapat.CsapatNev = csapat;
                csapatEredmenyei.Add(ujCsapat);
            }

            foreach (var cs in csapatEredmenyei)
            {
                cs.GyozelmekSzama = meccsek.Where(x => x.GyoztesCsapatNeve() == cs.CsapatNev).Count();
                cs.VeresegekSzama = meccsek.Where(x => x.HazaiCsapat == cs.CsapatNev || x.VendegCsapat == cs.CsapatNev && x.GyoztesCsapatNeve() !=
                    cs.CsapatNev && x.GyoztesCsapatNeve() != "").Count();
            }
        }
    }
}
