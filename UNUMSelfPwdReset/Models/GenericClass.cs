using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace UNUMSelfPwdReset.Models
{
    public class StaticMethods
    {
        
        public static PopupViewModel CreatePopupModel(string hdr, string msg)
        {
            PopupViewModel objPopup = new PopupViewModel();
            objPopup.Header = hdr;
            objPopup.Message = msg;

            return objPopup;
        }

        public static ResponseModel GetResponse(string strRes)
        {
            var res= Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel>(strRes);
            return res;
        }
    }
    public class SelectListItemObject
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class PopupViewModel
    {
        public string Dialog { get; set; }
        public string CssClassHeader { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }

        public List<PopupViewModelMessageItem> MessageItems { get; set; }
    }
    public class PopupViewModelMessageItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class SendEmailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<AttachmentFile> AttachmentFiles { get; set; }       

    }
    public class AttachmentFile
    {
        public byte [] DocFile { get; set; }
        public string FileName { get; set; }
    }

    public class MailLogModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentPath { get; set; }
        public int MailStatus { get; set; }
        public string InsertBy { get; set; }
        public string ProjectName { get; set; }
        public string ProcessName { get; set; }
        public string requestId { get; set; }
    }

    public class DropdownModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ResponceModel
    {
        public string id { get; set; }
        public string message { get; set; }
        public int? statusCode { get; set; }
    }
    public class PageIndexModel
    {
        public int totalRecords { get; set; }
        public int currentPage { get; set; }
        public int PageCount { get; set; }
        public int Startcount { get; set; }
        public int Endcount { get; set; }
    }
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
    }
}
