using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class updatecompany : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User currentUser = GetUser();

                if (currentUser == null)
                {
                    RedirectToHomeAndError("strUserNotLoggedIn");
                }

                CompanyManager companyManager = new CompanyManager();
                Company company = companyManager.GetCompany(currentUser.UserId);

                if (company == null)
                {
                    RedirectToHomeAndError("strUserHasNoCompany");
                }

                CountryManager countryManager = new CountryManager();
                ddlCompanyCountry.DataSource = countryManager.GetCountries();
                ddlCompanyCountry.DataBind();
                ddlCompanyCountry.SelectedValue = company.Country;

                ddlCompanyState.DataSource = countryManager.GetStates(ddlCompanyCountry.SelectedItem.Value);
                ddlCompanyState.DataBind();
                ddlCompanyState.SelectedValue = company.State;

                lblCompanyIsRecruiterDisplay.Text = (company.IsRecruiter? GetLocalResourceObject("lblCompanyIsRecruiterDisplayYes").ToString() : GetLocalResourceObject("lblCompanyIsRecruiterDisplayNo").ToString());
                lblCompanyNameDisplay.Text = company.Name;

                pnlEditCompany.Visible = false;
                pnlDisplayCompany.Visible = false;
                if (currentUser.IsCompanyAdmin)
                {
                    pnlEditCompany.Visible = true;
                    txtCompanyAddress1.Text = company.Address1;
                    txtCompanyAddress2.Text = company.Address2;
                    txtCompanyCity.Text = company.City;
                    txtCompanyZip.Text = company.Zip;
                    txtCompanyPhoneNumber.Text = company.Phone;
                    txtCompanyWebsite.Text = company.Website;
                }
                else
                {
                    pnlDisplayCompany.Visible = true;
                    lblDisplayCompanyAddress1.Text = company.Address1;
                    lblDisplayCompanyAddress2.Text = company.Address2;
                    lblDisplayCompanyCity.Text = company.City;
                    lblDisplayCompanyZip.Text = company.Zip;
                    lblDisplayCompanyPhone.Text = company.Phone;
                    lblDisplayCompanyWebsite.Text = company.Website;
                }
            }
        }

        protected void btnUpdateCompany_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                bool updateSuccess = false;
                User currentUser = GetUser();
                if (currentUser != null)
                {
                    CompanyManager companyManager = new CompanyManager();
                    updateSuccess = companyManager.UpdateCompany(currentUser.UserId,
                        txtCompanyAddress1.Text,
                        txtCompanyAddress2.Text,
                        txtCompanyCity.Text,
                        ddlCompanyState.SelectedValue,
                        txtCompanyZip.Text,
                        ddlCompanyCountry.SelectedValue,
                        txtCompanyPhoneNumber.Text,
                        txtCompanyWebsite.Text);
                }


                if (updateSuccess)
                {
                    this.AddSystemMessage(GetLocalResourceObject("strCompanyUpdateSuccess").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
                else
                {
                    this.AddSystemMessage(GetLocalResourceObject("strCompanyUpdateFailure").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
            else
            {
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();
            txtCompanyAddress1.Text = stringValidation.SanitizeUserInputString(txtCompanyAddress1.Text, StringValidation.SanitizeEntityNames.Address);
            txtCompanyAddress2.Text = stringValidation.SanitizeUserInputString(txtCompanyAddress2.Text, StringValidation.SanitizeEntityNames.Address);
            txtCompanyCity.Text = stringValidation.SanitizeUserInputString(txtCompanyCity.Text, StringValidation.SanitizeEntityNames.City);
            txtCompanyPhoneNumber.Text = stringValidation.SanitizeUserInputString(txtCompanyPhoneNumber.Text, StringValidation.SanitizeEntityNames.Phone);
            txtCompanyWebsite.Text = stringValidation.SanitizeUserInputString(txtCompanyWebsite.Text, StringValidation.SanitizeEntityNames.Url);
            Page.Validate("UpdateCompany");

            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}