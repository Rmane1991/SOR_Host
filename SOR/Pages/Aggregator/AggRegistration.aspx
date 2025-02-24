﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="AggRegistration.aspx.cs" Inherits="SOR.Pages.BC.AggRegistration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<script type="text/javascript" src="../../JavaScripts/FranchiseManagement.js"></script>--%>

    <script src="../../Scripts/aes.js"></script>
    <script src="../../Scripts/jquery-1.4.1-vsdoc.js"></script>
    <script src="../../Scripts/executeSec.js"></script>
    <script src="../../assets1/multi/jquery.min.js"></script>
    <style>
        span {
            font-size: 12px;
        }

        .err {
            color: #fe1a03;
        }

        .selectGrid-m-z {
            /* margin-top: 8px; */
            margin-bottom: 1px;
        }

        .col-md-99 {
            flex: -15 0 auto;
            width: 66%;
            margin-left: 49px;
        }

        .col-sm-77 {
            flex: 0 0 auto;
            width: 68%;
        }

        .select2 {
            width: 100% !important;
        }

        .col1 {
            width: 40%;
        }

        .col2 {
            width: 15%;
        }

        .col3 {
            width: 32%;
        }

        .col4 {
            width: 38%;
        }

        label {
            height: 20px;
        }
    </style>

    <style>
        .col-lg-3 {
            width: 20%;
        }

        .mb50 {
            margin-left: 277px;
        }
    </style>
    <style>
        .spanbtn {
            padding-top: 60px;
        }
    </style>
    <style>
        .iconButtonBox {
            margin-bottom: -11px;
        }
    </style>

    <script type="text/javascript">
        function onIdFileupload() {
            debugger;

            $(function () {
                var fileUpload = $('#<%=flgUplodMyIdProof.ClientID%>').get(0);

                var files = fileUpload.files;
                var test = new FormData();
                for (var i = 0; i < files.length; i++) {
                    test.append(files[i].name, files[i]);
                }
                $.ajax({
                    url: "IdFileupload.ashx",
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: test,
                    success: function (result) {
                        document.getElementById("<%=lbl_smsg.ClientID%>").innerText = "";
                        document.getElementById("<%=lbl_emsg.ClientID%>").innerText = "";

                        if (result.split(':')[0] == "File Uploaded Successfully") {
                            document.getElementById("<%=lbl_smsg.ClientID%>").innerText = result.split(':')[0];
                        } else {
                            document.getElementById("<%=lbl_emsg.ClientID%>").innerText = result.split(':')[0];
                            document.getElementById("<%= flgUplodMyIdProof.ClientID %>").innerHTML = "";
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            })
        }


    </script>

    <script>
        function checkAgreement() {
            debugger;
          <%--  chkValidaddress = document.getElementById('<%= chkValidaddress.ClientID %>');
            chkvaliddocs = document.getElementById('<%= chkvaliddocs.ClientID %>');--%>
            if (chkValidaddress.checked && chkvaliddocs.checked) {

            }
            else {
                alert('Please check confirmation for address and documents,.Declaration')

            }
        }

        function Confirm2() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Delete Data?")) {
                debugger;
                document.getElementById("<%= hdnDeleteConfirmation.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= hdnDeleteConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }
    </script>

    <script type="text/javascript">
        function SetValue(myval) {

            //var surl = "http://localhost:63840/pdfhandler.aspx?value="+myval+"";
            window.open("/pdfhandler.aspx?value=" + myval + "");
        }
    </script>
    <script type="text/javascript">
        function onAddfileupload() {
            debugger;

            $(function () {
                var fileUpload = $('#<%=flgUplodMyAddressProof.ClientID%>').get(0);

                var files = fileUpload.files;
                var test = new FormData();
                for (var i = 0; i < files.length; i++) {
                    test.append(files[i].name, files[i]);
                }
                $.ajax({
                    url: "AddFileUpload.ashx",
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: test,
                    success: function (result) {

                        document.getElementById("<%=lbl_smsg1.ClientID%>").innerText = "";
                        document.getElementById("<%=lbl_emsg1.ClientID%>").innerText = "";

                        if (result.split(':')[0] == "File Uploaded Successfully") {
                            document.getElementById("<%=lbl_smsg1.ClientID%>").innerText = result.split(':')[0];
                        } else {
                            document.getElementById("<%=lbl_emsg1.ClientID%>").innerText = result.split(':')[0];
                            document.getElementById("<%=flgUplodMyAddressProof.ClientID%>").value = "";
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            })
        }

    </script>

    <script type="text/javascript">

        function onSigfileupload() {
            debugger;

            $(function () {
                var fileUpload = $('#<%=flgUplodMySignatureProof.ClientID%>').get(0);

                var files = fileUpload.files;
                var test = new FormData();
                for (var i = 0; i < files.length; i++) {
                    test.append(files[i].name, files[i]);
                }
                $.ajax({
                    url: "SigFileUpload.ashx",
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: test,
                    success: function (result) {
                        document.getElementById("<%=lbl_smsg2.ClientID%>").innerText = "";
                        document.getElementById("<%=lbl_emsg2.ClientID%>").innerText = "";
                        if (result.split(':')[0] == "File Uploaded Successfully") {
                            document.getElementById("<%=lbl_smsg2.ClientID%>").innerText = result.split(':')[0];
                        } else {
                            document.getElementById("<%=lbl_emsg2.ClientID%>").innerText = result.split(':')[0];
                            document.getElementById("<%=flgUplodMySignatureProof.ClientID%>").value = "";
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            })
        }
    </script>

    <script type="text/javascript">

        function funShowOnboardDiv() {
            $("#divMainDetailsGrid").hide();
            $("#divlogoupdate").hide();
            $("#divOnboardFranchise").show();
            $("#divAction").hide();

        }
        function funShowgridDiv() {
            $("#divMainDetailsGrid").show();
            $("#divOnboardFranchise").hide();
            $("#divlogoupdate").hide();
            $("#divAction").show();
        }
        function funshowupdate() {
            $("#divMainDetailsGrid").hide();
            $("#divOnboardFranchise").hide();
            $("#divlogoupdate").show();
            $("#divAction").hide();
        }

        //function hidealldiv() {
        //      $("#divMainDetailsGrid").hide();
        //    $("#divOnboardFranchise").hide();
        //    $("#divlogoupdate").hide();
        //    $("#divAction").show();
        //}

        function Confirm() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Save Data?")) {
                debugger;
                document.getElementById("<%= HiddenField1.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= HiddenField1.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }
        function Confirm1() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Upload Your Data?")) {
                debugger;
                document.getElementById("<%= HiddenField2.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= HiddenField2.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }
        function Confirm3() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Save Your Data?")) {
                debugger;
                document.getElementById("<%= HiddenField3.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= HiddenField3.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }

        function FunImageFIllLogo(input) {
            debugger;
            var fileType = $('#MainContent_flgUplodMyImage').val().split('.').pop().toLowerCase();
            var flgUplodMyImage = document.getElementById('MainContent_flgUplodMyImage');

            // var fileType = input.value.split('.').pop().toLowerCase();
            var count = flgUplodMyImage.Filename.Split('.').Length - 1;
            if (fileType == 'jpg' || fileType == 'jpeg' || fileType == 'png') {
                if (input.files[0].size > 500000)//500 kb
                {
                    alert('File size should not exceed 500 KB in Attachments');
                    input.value = null;
                    return;
                }

                else if (count > '0') {
                    alert("upload Valid file");
                }

                else if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#MainContent_imageupload').attr('src', e.target.result);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }
            else {
                alert("File type should be only in jpg, jpeg and png  format");
                input.value = null;
                return;
            }
        }
        function FunImageUpdateLogo(input) {
            debugger;
            var fileType = $('#MainContent_filelogo').val().split('.').pop().toLowerCase();
            var filelogo = document.getElementById('MainContent_filelogo');

            // var fileType = input.value.split('.').pop().toLowerCase();
            var count = filelogo.Filename.Split('.').Length - 1;
            if (fileType == 'jpg' || fileType == 'jpeg' || fileType == 'png') {
                if (input.files[0].size > 500000)//500 kb
                {
                    alert('File size should not exceed 500 KB in Attachments');
                    input.value = null;
                    return;
                }

                else if (count > '0') {
                    alert("upload Valid file");
                }

                else if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#MainContent_imageupload').attr('src', e.target.result);
                    }

                    reader.readAsDataURL(input.files[0]);
                }
            }
            else {
                alert("File type should be only in jpg, jpeg and png  format");
                input.value = null;
                return;
            }
        }


        function Disable() {
            document.getElementById("MainContent_btnSubmitDetails").disabled = true;
        }

        function Clear(e) {
            e.preventDefault();
            document.getElementById("<%=txtFirstName.ClientID %>").value = '';

            document.getElementById("<%=txtMiddleName.ClientID %>").value = '';
            document.getElementById("<%=txtLastName.ClientID %>").value = '';
            document.getElementById("<%=txtFatherName.ClientID %>").value = '';

            document.getElementById("<%=txtEmailID.ClientID %>").value = '';
            document.getElementById("<%=txtPinCode.ClientID %>").value = '';


            document.getElementById("<%=txtRegisteredAddress.ClientID %>").value = '';

            document.getElementById("<%=txtContactNo.ClientID %>").value = '';
            document.getElementById("<%=txtLandlineNo.ClientID %>").value = '';

            document.getElementById("<%=txtLandlineNo.ClientID %>").value = '';

            document.getElementById("<%=ddlCity.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ddlCountry.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=ddlState.ClientID %>").selectedIndex = 0;

            document.getElementById("<%=txtAlterNateNo.ClientID %>").value = '';



        }


    </script>

    <script type="text/javascript">
        function FunRegisterDocuments(input) {
            debugger;
            var filename = $('#cpbdCarde_UploadForm').val().split('.').pop().toLowerCase();
            var fileInput = document.getElementById('cpbdCarde_UploadForm');
            if (filename == 'pdf') {
                var filesize = (fileInput.files[0].size);

                if (filesize > 2000000) {
                    showWarning('File size should not exceed 2 MB');
                    document.getElementById("#cpbdCarde_UploadForm").value = "";
                }
                else {
                    $('#MainContent_help_cpbdCarde_UploadForm').html('');
                }
            } else {
                showWarning('File Type should be .pdf format only');
                document.getElementById("cpbdCarde_UploadForm").value = "";
            }
        }
        function FunIdentityProof(input) {
            debugger;
            var filename = $('#cpbdCarde_flgUplodMyIdProof').val().split('.').pop().toLowerCase();
            var fileInput = document.getElementById('cpbdCarde_flgUplodMyIdProof');
            if (filename == 'jpg' || filename == 'jpeg' || filename == 'png' || filename == 'pdf') {
                var filesize = (fileInput.files[0].size);

                if (filesize > 2000000) {
                    showWarning('File size should not exceed 2 MB');
                    document.getElementById("#cpbdCarde_flgUplodMyIdProof").value = "";
                }
                else {
                    $('#MainContent_help_cpbdCarde_flgUplodMyIdProof').html('');
                }
            } else {
                showWarning('File Type should be .jpg,.jpeg,.png,.pdf format only');
                document.getElementById("cpbdCarde_flgUplodMyIdProof").value = "";
            }
        }
        function Funaddressproof(input) {
            debugger;
            var filename = $('#cpbdCarde_flgUplodMyAddressProof').val().split('.').pop().toLowerCase();
            var fileInput = document.getElementById('cpbdCarde_flgUplodMyAddressProof');
            if (filename == 'jpg' || filename == 'jpeg' || filename == 'png' || filename == 'pdf') {
                var filesize = (fileInput.files[0].size);

                if (filesize > 2000000) {
                    showWarning('File size should not exceed 2 MB');
                    document.getElementById("#cpbdCarde_flgUplodMyAddressProof").value = "";
                }
                else {
                    $('#MainContent_help_cpbdCarde_flgUplodMyAddressProof').html('');
                }
            } else {
                showWarning('File Type should be .jpg,.jpeg,.png,.pdf format only');
                document.getElementById("cpbdCarde_flgUplodMyAddressProof").value = "";
            }
        }
        function FunsignatureProof(input) {
            debugger;
            var filename = $('#cpbdCarde_flgUplodMySignatureProof').val().split('.').pop().toLowerCase();
            var fileInput = document.getElementById('cpbdCarde_flgUplodMySignatureProof');
            if (filename == 'jpg' || filename == 'jpeg' || filename == 'png' || filename == 'pdf') {
                var filesize = (fileInput.files[0].size);

                if (filesize > 2000000) {
                    showWarning('File size should not exceed 2 MB');
                    document.getElementById("#cpbdCarde_flgUplodMySignatureProof").value = "";
                }
                else {
                    $('#MainContent_help_cpbdCarde_flgUplodMySignatureProof').html('');
                }
            } else {
                showWarning('File Type should be .jpg,.jpeg,.png,.pdf format only');
                document.getElementById("cpbdCarde_flgUplodMySignatureProof").value = "";
            }
        }
    </script>

    <script>
        function onClientSelectedIndexChanged() {
            debugger;
            ddlRegisteredtype = document.getElementById("<%=ddlIdentityProof.ClientID %>").value;
            if (ddlRegisteredtype == "--Select--") {
                $("#<%=divIdProof.ClientID %>").hide();
                $("#<%=dvfield_PANNos.ClientID %>").hide();
                document.getElementById("<%=ddlSignature.ClientID %>").disabled = false;
                document.getElementById("<%=ddlAddressProof.ClientID %>").disabled = false;

          <%--      document.getElementById("<%=RequiredFieldValidator1.ClientID %>").enabled = true;--%>

                document.getElementById('<%=CheckBoxAddress.ClientID%>').checked = false;
                return false;
            }
            else {
                $("#<%=dvfield_PANNos.ClientID %>").show();
                $("#<%=divIdProof.ClientID %>").show();
            }
        }


        function onClientSignatureSelectedIndexChanged() {
            debugger;
            ddlRegisteredtype = document.getElementById("<%=ddlSignature.ClientID %>").value;

            if (ddlRegisteredtype == "--Select--") {
                $("#<%=divSigProof.ClientID %>").hide();
                alert('Please select Signature Proof.');
                return false;
            }
            else {
                $("#<%=divSigProof.ClientID %>").show();
            }
        }
        function onClientAddressSelectedIndexChanged() {
            debugger;
            ddlRegisteredtype = document.getElementById("<%=ddlAddressProof.ClientID %>").value;
            if (ddlRegisteredtype == "--Select--") {
                $("#<%=divAddressProof.ClientID %>").hide();
                document.getElementById("flgUplodMyAddressProof").value = '';
                alert('Please select Address Proof.');
                return false;
            }
            else {
                $("#<%=divAddressProof.ClientID %>").show();
            }
        }
    </script>

    <script type="text/javascript">

        function funShowPopup(id) {
            debugger;
            // $("#popBulkFranchiseInsert").modal();
            // $("#popBulkFranchiseInsert").modal("show");
            $('#popBulkFranchiseInsert').show();

        }

    </script>

    <script type="text/javascript">
        function show(oLink, targetDivID) {
            var arrDIVs = oLink.parentNode.getElementsByTagName("div");
            for (var i = 0; i < arrDIVs.length; i++) {
                var oCurDiv = arrDIVs[i];
                if (oCurDiv.id.indexOf(targetDivID) >= 0) {
                    var blnHidden = (oCurDiv.style.display == "none");
                    oCurDiv.style.display = (blnHidden) ? "block" : "none";
                }
            }
            return false;
        }
    </script>
    <style>
        .pagination-ys {
            color: black;
            background-color: #11AEA3;
            border: 1px solid #dddddd;
            margin-left: -1px;
            font-family: Arial Black;
        }



        .form-group {
            margin-bottom: 15px;
        }



        .GridView {
            width: 100%;
            height: auto;
            overflow-y: auto;
            border: solid 0px #B9B9B9 !important;
        }



            .GridView table {
                width: 1%;
                /*border-collapse: collapse;*/
            }



            .GridView th {
                background: #FFB2B2;
                color: black;
                font-weight: bold !important;
                padding: 4px 20px 4px 4px !important;
            }



            .GridView td {
                background: none;
                color: #420629;
                padding: 5px;
                border: 1px solid #CCC;
            }




        .select-grid-gap.row > * {
            margin-bottom: 10px;
        }



        .GridView tr:nth-child(even) {
            background: #eaecff !important;
        }



        .GridView tr:nth-child(odd) {
            background: #FFF !important;
        }



        .table tbody tr th {
            background: none;
            color: #fbd2ce;
        }



        th {
            background-color: #fbd2ce;
            color: black;
        }



        .col-md-2 {
            flex: 0 0 auto;
            width: 12.666667%;
        }
    </style>
    <style>
        .HeaderStyle table tbody tr th {
            background-color: #fbd2ce !important;
            font-weight: 400;
            position: sticky;
            white-space: nowrap;
            width: auto;
            z-index: 100;
            top: -8px;
        }
    </style>
    <style type="text/css">
        .btnCommonCSS {
            height: 30px;
            margin-top: 8px;
        }



        .GridView tr:nth-child(odd) {
            background: #202565;
            color: #FFF;
        }



        .form-group {
            margin-bottom: 5px;
        }



        .btnClass {
            margin: 5px;
        }
    </style>

    <style>
        #ctl00_cpbdCarde_gvsummary {
            width: 100% !Important;
        }

        .adjustMarginTB {
            margin: 10px 0px 0px 0px;
            height: 30px;
        }

        .select2 {
            width: 100% !important;
        }

        .flex {
            width: 100% !important;
        }

        fieldset.scheduler-border {
            border: solid 1px lightgray !important;
            border-radius: 5px;
            padding: 0 0px 10px 5px;
            font-size: small;
            border-bottom: none;
            overflow-y: hidden;
            overflow-x: hidden;
        }

        legend.scheduler-border {
            width: auto !important;
            border: none;
            font-size: small;
            overflow-y: hidden;
            overflow-x: hidden;
            color: #1E3C6E;
            font-weight: bold;
        }

        /*Calendar Control CSS*/
        .cal_Theme1 .ajax__calendar_container {
            background-color: #AFC7F7;
            border: solid 1px #0c2d50;
            z-index: 999;
        }

        .cal_Theme1 .ajax__calendar_header {
            background-color: #ffffff;
            margin-bottom: 4px;
        }

        .cal_Theme1 .ajax__calendar_title, .cal_Theme1 .ajax__calendar_next, .cal_Theme1 .ajax__calendar_prev {
            color: #004080;
            padding-top: 1px;
        }

        .cal_Theme1 .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px black;
            height: 130px;
            width: 160px;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            color: #004080;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            text-align: center;
        }

        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_day, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_month, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_year, .cal_Theme1 .ajax__calendar_active {
            color: #004080;
            font-weight: bold;
            background-color: #DEF1F4;
        }

        .cal_Theme1 .ajax__calendar_today {
            font-weight: bold;
        }

        .cal_Theme1 .ajax__calendar_other, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_today, .cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
            color: #bbbbbb;
        }
    </style>

    <style type="text/css">
        .radioButtonList input {
            display: inline-block;
            margin-right: 1em;
        }

        .radioButtonList label {
            display: inline-block;
            margin-right: 1.25em;
        }
    </style>

    <script>
        function fntab1ValicationOnddlDataStatusInsert(e, t) {
            debugger;
            e.preventDefault();
            ExcelFileuploadInsert = document.getElementById("<%=ExcelFileuploadInsert.ClientID %>").value;
            if (ExcelFileuploadInsert == "") {
                alert('Please select file.');
                document.getElementById("<%=ExcelFileuploadInsert.ClientID%>").focus();
                return false;
            }
            else {
                __doPostBack('ctl00$MainContent$btnUploadBulkTask', '');
            }
        }
    </script>

    <script>
        function maskInputs() {
            if (document.getElementById("<%=txtPANNo.ClientID %>") != "" && document.getElementById("<%=txtPANNo.ClientID %>") != null) {
                var result = "";
                var lblValue = document.getElementById("<%=txtPANNo.ClientID %>");
                var newpwd = document.getElementById("<%=txtPANNo.ClientID %>");
                newpwd.value = fetchAscii(lblValue.value);
                var Data = document.getElementById("<%=hidPan.ClientID %>");
                Data.value = newpwd.value;
                result = Encry(document.getElementById("<%=hidPan.ClientID %>").value);
                document.getElementById("<%=hidPan.ClientID %>").value = result;
                document.getElementById("<%=txtPANNo.ClientID %>").value = '';
            }
            if (document.getElementById("<%=txtaadharno.ClientID %>") != "" && document.getElementById("<%=txtaadharno.ClientID %>") != null) {
                var result = "";
                var lblValueU = document.getElementById("<%=txtaadharno.ClientID %>");
                var newpwdU = document.getElementById("<%=txtaadharno.ClientID %>");
                newpwdU.value = fetchAscii(lblValueU.value);
                var Data = document.getElementById("<%=hidAadh.ClientID %>");
                Data.value = newpwdU.value;
                result = Encry(document.getElementById("<%=hidAadh.ClientID %>").value);
                document.getElementById("<%=hidAadh.ClientID %>").value = result;
                document.getElementById("<%=txtaadharno.ClientID %>").value = '';
            }
            if (document.getElementById("<%=txtGSTNo.ClientID %>") != "" && document.getElementById("<%=txtGSTNo.ClientID %>") != null) {
                var result = "";
                var lblValueU = document.getElementById("<%=txtGSTNo.ClientID %>");
                var newpwdU = document.getElementById("<%=txtGSTNo.ClientID %>");
                newpwdU.value = fetchAscii(lblValueU.value);
                var Data = document.getElementById("<%=hidSgst.ClientID %>");
                Data.value = newpwdU.value;
                result = Encry(document.getElementById("<%=hidSgst.ClientID %>").value);
                document.getElementById("<%=hidSgst.ClientID %>").value = result;
                document.getElementById("<%=txtGSTNo.ClientID %>").value = '';
            }

            if (document.getElementById("<%=txtAccountNumber.ClientID %>") != "" && document.getElementById("<%=txtAccountNumber.ClientID %>") != null) {
                var result = "";
                var lblValueU = document.getElementById("<%=txtAccountNumber.ClientID %>");
                var newpwdU = document.getElementById("<%=txtAccountNumber.ClientID %>");
                newpwdU.value = fetchAscii(lblValueU.value);
                var Data = document.getElementById("<%=hidAccNo.ClientID %>");
                Data.value = newpwdU.value;
                result = Encry(document.getElementById("<%=hidAccNo.ClientID %>").value);
                document.getElementById("<%=hidAccNo.ClientID %>").value = result;
                document.getElementById("<%=txtAccountNumber.ClientID %>").value = '';
            }
            if (document.getElementById("<%=txtIFsccode.ClientID %>") != "" && document.getElementById("<%=txtIFsccode.ClientID %>") != null) {
                var result = "";
                var lblValueU = document.getElementById("<%=txtIFsccode.ClientID %>");
                var newpwdU = document.getElementById("<%=txtIFsccode.ClientID %>");
                newpwdU.value = fetchAscii(lblValueU.value);
                var Data = document.getElementById("<%=hidAccIFC.ClientID %>");
                Data.value = newpwdU.value;
                result = Encry(document.getElementById("<%=hidAccIFC.ClientID %>").value);
                document.getElementById("<%=hidAccIFC.ClientID %>").value = result;
                document.getElementById("<%=txtIFsccode.ClientID %>").value = '';
            }
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#RadioConfigCategory").change(function () {
                var sel = $('#RadioConfigCategory input:checked').val()
                if (sel == "1") {
                    $("#DivMinAmount").hide();

                }
                if (sel == "2") {
                    $("#DivMinAmount").show();
                }
            });
            return false;
        });

    </script>

    <script type="text/javascript">
        function disableMultipleClick() {
            if (Page_ClientValidate("FranchiseReg")) {
                debugger;
                debugger;
                $("#div_Upload").hide();
                $("#DivBcDetails").show();
                document.getElementById("cpbdCarde_btnSubmitDetails").disabled = true;
                document.getElementById("cpbdCarde_btnSubmitDetails").innerHTML = "Processing";
                __doPostBack('ctl00$cpbdCarde$btnSubmitDetails', '');
            }

            Page_BlockSubmit = false;
            return false;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphmastermain" runat="server">
    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
    </asp:Panel>
    <div style="clear: both;" />
    <asp:HiddenField ID="hidPan" runat="server" />
    <asp:HiddenField ID="hidAadh" runat="server" />
    <asp:HiddenField ID="hidSgst" runat="server" />
    <asp:HiddenField ID="hidAccIFC" runat="server" />
    <asp:HiddenField ID="hidAccNo" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:HiddenField ID="hdnDeleteConfirmation" runat="server" />
    <div class="container-fluid">
        <div class="breadHeader">
            <h5 class="page-title">Aggregator Registration</h5>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div id="divAction" runat="server">

                    <div class="accordion summary-accordion" id="history-accordionss">
                        <div class="accordion-item">
                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOns">
                                <h6 class="searchHeader-heading"></h6>
                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOns"
                                    aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-hide"></span>
                                    <p>Show / Hide</p>
                                </button>
                            </div>
                            <div id="collapseSummaryOns" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                data-bs-parent="#summary-accordion">
                                <div class="accordion-body">
                                    <hr class="hr-line">

                                    <span class="spanbtn">
                                        <asp:Button ID="btnAddnew" runat="server" Text="Add New" CssClass="themeBtn themeApplyBtn" Width="102px" Height="32px" OnClick="btnAddnew_Click" />
                                    </span>

                                    <%--OnClick="btndownload_ServerClick"--%>
                                    <hr class="hr-line">
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Request Type</label>
                                            <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Registration" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Activation" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="De-activation" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Termination" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="ReEdit" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <%--<div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Bucket</label>
                                            <asp:DropDownList ID="ddlBucketId" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Self" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Level -1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Level -2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Level -3" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>--%>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">Request Status</label>
                                            <asp:DropDownList ID="ddlRequestStatus" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Declined" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <%-- <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">PanNo</label>
                                            <div class="inputBox w-100">
                                                <input type="text" id="txtPanNoF" runat="server" class="input-text form-control" style="width: 100%" placeholder="Pan No" />
                                            </div>
                                        </div>--%>
                                        <div class="col">
                                            <label class="selectInputLabel" for="selectInputLabel">PAN No.</label>
                                            <div class="inputBox w-100">
                                                <%--<input type="text" id="txtPanNoF" runat="server" class="input-text form-control" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" />--%>
                                                <%--<asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPanNoF" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" oninput="this.value = this.value.toUpperCase();"></asp:TextBox>
                                                <asp:HiddenField ID="hd_txtPanNoF" runat="server" Value="1" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPanNoF" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender52" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtPanNoF" />--%>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtPanNoF" Style="display: none" runat="server" CssClass="err" ValidationGroup="FranchiseReg" ErrorMessage="Please enter pan no"></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPanNoF" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" oninput="this.value = this.value.toUpperCase();"></asp:TextBox>
                                                <asp:HiddenField ID="HiddenField4" runat="server" Value="1" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPanNoF" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="UppercaseLetters,Numbers, LowercaseLetters" TargetControlID="txtPanNoF" />
                                            </div>
                                        </div>

                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                            <div>
                                                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" class="themeBtn themeApplyBtn" Text="Search"></asp:Button>
                                                <asp:Button ID="butnCancel" runat="server" CausesValidation="false" class="themeBtn resetBtn themeCancelBtn me-0" Text="Clear" OnClick="butnCancel_Click"></asp:Button>
                                            </div>
                                        </div>
                                        <div class="col-auto ms-auto d-flex justify-content-end">
                                        </div>
                                        <div class="row d-flex justify-content-center align-items-center">

                                            <div class="d-flex justify-content-end">
                                                <asp:ImageButton ID="btnExportCSV" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                    ToolTip="Csv" data-toggle="modal" data-target="#myModal" OnClick="btnExportCSV_Click" />
                                                <asp:ImageButton ID="btndownload" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                    ToolTip="Xls" data-toggle="modal" data-target="#myModal" OnClick="btndownload_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div id="divMainDetailsGrid" runat="server">
                    <asp:UpdatePanel ID="updCommen" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidFranchiseID" runat="server" />
                            <asp:HiddenField ID="hidEmailId" runat="server" />
                            <asp:HiddenField ID="hidCashLimit" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel ID="panelGrid" runat="server" HorizontalScroll="false" ScrollBars="None" Style="padding: 5px 10px 0px 0px;">
                                    <div class="form-group row">
                                        <div class="tableBorderBox HeaderStyle" style="padding: 10px 10px;">
                                <div class="table-box">
                                    <asp:GridView ID="gvBCOnboard" runat="server"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        AllowPaging="true"
                                        CssClass="GridView"
                                        Visible="true"
                                        PageSize="10"
                                        PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageText="First Page"
                                        PagerSettings-LastPageText="Last Page"
                                        OnRowCommand="gvBCOnboard_RowCommand"
                                        DataKeyNames="Bucket,RequestStatus,RequestType,ActivityType,ReqId"
                                        OnPageIndexChanging="gvBCOnboard_PageIndexChanging"
                                        OnRowDataBound="gvBCOnboard_RowDataBound">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                        <RowStyle Wrap="false" />
                                        <AlternatingRowStyle BackColor="#F1FCEE" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="background: #FBD2CE; color: black; border: none;">
                                                                <label>Action </label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="display: flex; justify-content: center; align-items: center;">
                                                    <%-- CommandArgument='<%#Eval("BC Code")+","+ Eval("numBCID")%>'--%>
                                                    <asp:ImageButton ID="btnView" runat="server" ImageUrl="../../images/Edit-01-512.png" Width="20px" Height="20px" CommandName="EditDetails"
                                                        ToolTip="Click Here To Edit Data" CommandArgument='<%#Eval("ReqId")+"="+Eval("Bucket")%>' data-toggle="modal" data-target="#ModalBC" />
                                                    <%--OnClick="btnView_Click"--%>
                                                    <asp:ImageButton ID="btndelete" runat="server" ImageUrl="../../images/document-delete.png" OnClientClick="Confirm2();" Width="20px" Height="20px" CommandName="DeleteDetails"
                                                        ToolTip="Click Here To Delete Data" data-toggle="modal" data-target="#ModalBC" />
                                                        <asp:ImageButton ID="btnSuccess" runat="server" ImageUrl="../../images/Sucessfull.png" Width="20px" Height="20px"
                                                                ToolTip="Done" data-toggle="modal" data-target="#ModalBC"/>
                                                                 </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--   <asp:BoundField DataField="Sr.No." HeaderText="Sr.No." />--%>
                                            <asp:BoundField DataField="aggCode" HeaderText="Code" />
                                            <asp:BoundField DataField="ReqId" HeaderText="Aggregator ID" Visible="false" />
                                            <asp:BoundField DataField="aggName" HeaderText="Name" />
                                            <asp:BoundField DataField="aggAddress" HeaderText="Address" />
                                            <asp:BoundField DataField="PAN No." HeaderText="PAN No." />
                                            <asp:BoundField DataField="AadharNo" HeaderText="Aadhar No." />
                                            <asp:BoundField DataField="ContactNo" HeaderText="Mobile No." />
                                            <asp:BoundField DataField="RequestType" HeaderText="Type" />
                                            <asp:BoundField DataField="Bucket" HeaderText="Bucket" Visible="false" />
                                            <asp:BoundField DataField="RequestStatus" HeaderText="Status" />
                                            <asp:BoundField DataField="ActivityType" HeaderText="ActivityType" Visible="false" />
                                        </Columns>
                                        <HeaderStyle BackColor="#8DCCF6" ForeColor="#3D62B6" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="row backImg" id="divOnboardFranchise" runat="server" visible="false">

                    <div id="DIVDetails" runat="server" visible="false">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                </div>
                                <div class="clearfix"></div>
                                <div class="panel-body">
                                    <div class="accordion summary-accordion" id="history-accordion">
                                        <div class="accordion-item">
                                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOne">
                                                <h6 class="searchHeader-heading">General Information</h6>
                                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOne"
                                                    aria-expanded="true" aria-controls="collapseOne">
                                                    <span class="icon-hide"></span>
                                                    <p>Show / Hide</p>
                                                </button>
                                            </div>

                                            <div id="collapseSummaryOne" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                                data-bs-parent="#summary-accordion">
                                                <div class="accordion-body">
                                                    <hr class="hr-line">
                                                    <!-- grid -->
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                        <div class="col">
                                                            <label class="exampleInputEmail1" for="selectInputLabel">Business Correspondence:<span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ControlToValidate="ddlbcCode" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select BC."></asp:RequiredFieldValidator>

                                                            <div class="selectInputDateBox w-100">
                                                                <asp:DropDownList ID="ddlbcCode" runat="server" CssClass="maximus-select w-100" Width="100%" OnSelectedIndexChanged="ddlbcCode_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="0">--Business Correspondents--</asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>

                                                        <div class="col" id="Div_Chk" runat="server" style="width: 800px">
                                                            <asp:Label ID="lblServicesOffer" runat="server" Text="Services offered "></asp:Label><span class="err">*</span><br />

                                                            <asp:CheckBox ID="chkAEPS" runat="server" Style="font-weight: 500" />
                                                            <asp:Label ID="lblchkAEPS" runat="server">AePS &emsp;</asp:Label>

                                                            <asp:CheckBox ID="chkdmt" runat="server" />
                                                            <asp:Label ID="lblchkdmt" runat="server">DMT &emsp;</asp:Label>

                                                            <asp:CheckBox ID="chkMATM" runat="server" />
                                                            <asp:Label ID="lblchkMATM" runat="server">Micro ATM &emsp;</asp:Label>
                                                        </div>

                                                        <div class="col" style="display: none" runat="server" visible="false">
                                                            <label for="exampleInputEmail1">BC Profile Image  <span class="err">*</span></label>
                                                            <asp:Image ID="imgMyImge" runat="server" Height="110px" Width="110px" BorderStyle="Double" ImageUrl="../../../images/Profile1.png" />
                                                            <asp:FileUpload ID="flgUplodMyImage" Width="120px" runat="server" Style="margin-top: 5px" onchange="return FunImageFIll(this);" CssClass="btn btn-small btn-default" />
                                                            <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <br />

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Aggregator Name <span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="txtFirstName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFirstName" PlaceHolder="Aggregator Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtFirstName" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtFirstName" />
                                                        </div>
                                                        <div class="col" runat="server" visible="false">
                                                            <label for="exampleInputEmail1">Middle Name </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtMiddleName" PlaceHolder="Middle Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtMiddleName" />
                                                        </div>
                                                        <div class="col" runat="server" visible="false">
                                                            <label for="exampleInputEmail1">Last Name </label>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtLastName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>--%>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLastName" PlaceHolder="Last Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtLastName" />
                                                        </div>
                                                        <div class="col" style="display: none">
                                                            <label for="exampleInputEmail1">Father Name </label>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldlValidator2" ControlToValidate="txtFatherName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Father name"></asp:RequiredFieldValidator>--%>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFatherName" PlaceHolder="Father Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtFatherName" />

                                                        </div>
                                                        <div class="col" style="display: none">
                                                            <label for="exampleInputEmail1">Gender </label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlGender" Width="100%">
                                                                <asp:ListItem Value="Male">Male</asp:ListItem>
                                                                <asp:ListItem Value="Female">Female</asp:ListItem>
                                                                <asp:ListItem Value="Transgender">Transgender</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col" id="dvfield_PANNo" runat="server" style="display: normal;">
                                                            <label for="exampleInputEmail1">PAN No. <span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtPANNo" Style="display: none" runat="server" CssClass="err" ValidationGroup="FranchiseReg" ErrorMessage="Please enter pan no"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPANNo" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" oninput="this.value = this.value.toUpperCase();"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtPANNo" runat="server" Value="1" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPANNo" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="UppercaseLetters,Numbers, LowercaseLetters" TargetControlID="txtPANNo" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">GST No. </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtGSTNo" MaxLength="15" Width="100%" PlaceHolder="Enter 15 digit GST no."></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtGSTNo" runat="server" Value="1" />
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Aadhaar card No. <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtaadharno" Width="100%" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12" onkeypress="return isNumber(event)"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtaadharno" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" ControlToValidate="txtaadharno" Display="None" runat="server" CssClass="err" ValidationGroup="FranchiseReg" ErrorMessage="Please enter Aadhaarcard no"></asp:RequiredFieldValidator>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" FilterType="Numbers" TargetControlID="txtaadharno" />
                                                        </div>
                                                        <div class="col">
                                                            <label class="selectInputEdu" for="selectInputLabel">Edu. Qualification <span class="err">*</span> </label>
                                                            <asp:DropDownList runat="server" class="maximus-select w-100" ID="ddleducation">
                                                                <asp:ListItem Value="Under 10th">Under 10th</asp:ListItem>
                                                                <asp:ListItem Value="10th">10th</asp:ListItem>
                                                                <asp:ListItem Value="12th">12th</asp:ListItem>
                                                                <asp:ListItem Value="Graduate">Graduate</asp:ListItem>
                                                                <asp:ListItem Value="Post Graduate">Post Graduate</asp:ListItem>
                                                                <asp:ListItem Value="Diploma Qualification">Diploma Qualification</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">



                                                        <div class="col" style="display:none">
                                                            <label for="exampleInputEmail1">BC Category </label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlCategory" Width="100%">
                                                                <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="General">General</asp:ListItem>
                                                                <asp:ListItem Value="OBC">OBC</asp:ListItem>
                                                                <asp:ListItem Value="SC">SC</asp:ListItem>
                                                                <asp:ListItem Value="ST">ST</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Type Of Organization </label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="DDlOrg" Width="100%">
                                                                <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="IT">IT</asp:ListItem>
                                                                <asp:ListItem Value="Agricultural">Agricultural</asp:ListItem>
                                                                <asp:ListItem Value="Commercial">Commercial</asp:ListItem>
                                                                <%--     <asp:ListItem Value="ST">ST</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputAccountNumber">Account Number <span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator36" ControlToValidate="txtAccountNumber" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Account Number"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAccountNumber" Width="100%" PlaceHolder="Account Number" MaxLength="19"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtAccountNumber" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" FilterType="Numbers" TargetControlID="txtAccountNumber" />
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputIFSCode">IFSC Code<span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator35" ControlToValidate="txtIFsccode" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter IFSC Code"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtIFsccode" PlaceHolder="IFSC Code" Width="100%" MaxLength="11"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtIFsccode" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtIFsccode" />
                                                        </div>

                                                    </div>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    </div>

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div style="height: 10px"></div>

                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                </div>
                                <div class="clearfix"></div>
                                <div class="panel-body">
                                    <div class="accordion summary-accordion" id="history-accordions">
                                        <div class="accordion-item">
                                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes">
                                                <h6 class="searchHeader-heading">Communication Details</h6>
                                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOnes"
                                                    aria-expanded="true" aria-controls="collapseOne">
                                                    <span class="icon-hide"></span>
                                                    <p>Show / Hide</p>
                                                </button>
                                            </div>

                                            <div id="collapseSummaryOnes" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                                data-bs-parent="#summary-accordion">
                                                <div class="accordion-body">
                                                    <hr class="hr-line">
                                                    <!-- grid -->

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                        <div class="col1">
                                                            <label for="exampleInputEmail1">Registered Address <span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtRegisteredAddress" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter registered address"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" class="form-control" ID="txtRegisteredAddress" TextMode="MultiLine" Width="100%" PlaceHolder="Registered Address " Style="resize: none"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtRegisteredAddress" runat="server" Value="1" />
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Pincode <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" PlaceHolder="eg.400601" Width="100%" ID="txtPinCode" MaxLength="6" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtPinCode" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" FilterType="Numbers" TargetControlID="txtPinCode" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtPinCode" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter pincode"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col" style="display: none">
                                                            <label for="exampleInputEmail1">Country <span class="err">*</span></label>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" ControlToValidate="ddlCountry" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select country"></asp:RequiredFieldValidator>--%>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlCountry" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                                            <%--OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"--%>
                                                            <asp:HiddenField ID="hd_ddlCountry" runat="server" Value="1" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">State <span class="err">*</span></label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlState" Width="100%" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <%--OnSelectedIndexChanged="ddlState_SelectedIndexChanged"--%>
                                                            <asp:HiddenField ID="hd_ddlState" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="ddlState" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select state"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">District<span class="err">*</span></label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlDistrict" Width="100%" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <%--<asp:TextBox runat="server" CssClass="input-text form-control" ID="txtDistrict" Width="100%" PlaceHolder="Enter district" MaxLength="25"></asp:TextBox>--%>
                                                            <asp:HiddenField ID="hd_ddlDistrict" runat="server" Value="1" />
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="ddlDistrict" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter district Name "></asp:RequiredFieldValidator>--%>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="ddlDistrict" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select state"></asp:RequiredFieldValidator>
                                                            <%--<Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="LowercaseLetters, UppercaseLetters" TargetControlID="ddlDistrict" />--%>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">City <span class="err">*</span></label>
                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" Width="100%" ID="ddlCity"></asp:DropDownList>
                                                            <asp:HiddenField ID="hd_ddlCity" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ControlToValidate="ddlCity" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select city"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Personal Email Id <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtEmailID" PlaceHolder="Email ID" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="txtEmailID" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Personal email id"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="validateEmail" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Please enter valid Personal email id" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers,UppercaseLetters, LowercaseLetters, Custom" ValidChars=".@!#$%^&*()_,/\-" TargetControlID="txtEmailID" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Contact No. <span class="err">*</span></label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtContactNo" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtContactNo" runat="server" Value="1" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtContactNo" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter contact no"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '6/7/8/9'." ControlToValidate="txtContactNo" ValidationExpression="^[6789]\d{9}$" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers" TargetControlID="txtContactNo" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Landline No. </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLandlineNo" PlaceHolder="Enter landline no." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtLandlineNo" runat="server" Value="1" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers" TargetControlID="txtLandlineNo" />
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Alternate No </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAlterNateNo" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                            <asp:HiddenField ID="hd_txtAlterNateNo" runat="server" Value="0" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '7/8/9'." ControlToValidate="txtAlterNateNo" ValidationExpression="^[789]\d{9}$" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" FilterType="Numbers" TargetControlID="txtAlterNateNo" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                <div>
                                                    <asp:Button runat="server" ID="btnSubmitDetails" ValidationGroup="FranchiseReg" OnClientClick="Confirm();" OnClick="btnSubmitDetails_Click" Class="themeBtn themeApplyBtn" Text="Submit"></asp:Button>
                                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false" class="themeBtn resetBtn themeCancelBtn me-0" Text="Back" OnClick="btnCancel_Click"></asp:Button>
                                                </div>

                                            </div>
                                            <asp:ValidationSummary
                                                HeaderText="You must enter or select a value in the following fields:"
                                                DisplayMode="BulletList"
                                                EnableClientScript="true"
                                                CssClass="err"
                                                ShowMessageBox="true"
                                                ShowSummary="false"
                                                ForeColor="Red"
                                                ValidationGroup="FranchiseReg"
                                                runat="server" />
                                        </div>
                                    </div>


                                </div>


                            </div>
                        </div>
                    </div>

                </div>
                <div style="height: 10px"></div>

                <div id="div_Upload" runat="server" visible="false">
                    <%--<asp:Panel ID="Panel2" runat="server" Width="100%" Style="margin: 5px 0px 0px 0px; padding: 10px; border: 1px solid gray;">--%>
                    <div class="accordion summary-accordion" id="history-accordion1">
                        <div class="accordion-item">
                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummarytwo">
                                <h6 class="searchHeader-heading">DOCUMENT UPLOAD</h6>
                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummarytwo"
                                    aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-hide"></span>
                                    <p>Show / Hide</p>
                                </button>
                            </div>

                            <div id="collapseSummarytwo" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                data-bs-parent="#summary-accordion">
                                <div class="accordion-body">
                                    <hr class="hr-line">

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-z select-grid-gap" runat="server" visible="false">
                                        <div class="col">
                                            <label for="exampleInputEmail1"><b>Registration Form</b></label>
                                            <br />
                                            <button type="button" class="btn btn-success" id="Button1" runat="server">Download Sample Form</button>
                                            <%--onserverclick="btndownload_ServerClick"--%>
                                        </div>
                                        <div class="col-md-6 col-xm-12">
                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                            <asp:FileUpload ID="UploadForm" CssClass="btn btn-small btn-default form-control" runat="server" AllowMultiple="false" Width="95%" onchange="return FunImageUpdateLogo(this);" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="None" ControlToValidate="UploadForm" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select Registration form"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-z select-grid-gap" runat="server" visible="false">
                                        <label style="font-weight: 200">Note:Download the Registration Form and upload here after fill up.</label>
                                    </div>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-z select-grid-gap">
                                        <div class="col1">
                                            <label for="exampleInputEmail1" style="width: 200px">Identity Proof.<span class="err">*</span></label>
                                            <asp:DropDownList runat="server" CssClass="maximus-select w-100" ID="ddlIdentityProof" onchange="onClientSelectedIndexChanged()">
                                                <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                                <asp:ListItem Value="Pancard">Pancard</asp:ListItem>
                                                <asp:ListItem Value="VoterId">VoterId</asp:ListItem>
                                                <asp:ListItem Value="Passport">Passport</asp:ListItem>
                                                <asp:ListItem Value="Aadhaarcard">Aadhaarcard</asp:ListItem>
                                                <asp:ListItem Value="Driverlicense">Driverlicense</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldIdentityProof" Display="None" InitialValue="--Select--" ControlToValidate="ddlIdentityProof" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select IdentityProof "></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 col-xm-12" runat="server" id="divIdProof" style="display: none;">
                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                            <asp:FileUpload ID="flgUplodMyIdProof" CssClass="btn btn-small btn-default form-control" onchange="onIdFileupload();" runat="server" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                                            <br />
                                            <asp:Label ID="lbl_emsg" runat="server" ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lbl_smsg" runat="server" ForeColor="Green"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="None" ControlToValidate="flgUplodMyIdProof" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select IdentityProof document"></asp:RequiredFieldValidator>
                                        </div>

                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-z select-grid-gap">
                                        <div class="col" id="dvfield_PANNos" runat="server" visible="false">
                                            <label for="exampleInputEmail1">Apply For All Related Proofs. </label>
                                            <asp:CheckBox ID="CheckBoxAddress" runat="server" onchange="javascript:ShowAddress();" />
                                        </div>
                                    </div>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-z select-grid-gap">
                                        <div class="col1">
                                            <label for="exampleInputEmail1" style="width: 200px">Address Proof.<span class="err">*</span></label>
                                            <asp:DropDownList runat="server" CssClass="maximus-select w-100" ID="ddlAddressProof" onchange="onClientAddressSelectedIndexChanged()">
                                                <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                                <asp:ListItem Value="VoterId">VoterId</asp:ListItem>
                                                <asp:ListItem Value="Passport">Passport</asp:ListItem>
                                                <asp:ListItem Value="Aadhaarcard">Aadhaarcard</asp:ListItem>
                                                <asp:ListItem Value="PhoneBillOnlyMTNLandBSNL">PhoneBillOnlyMTNLandBSNL</asp:ListItem>
                                                <asp:ListItem Value="Driverlicense">Driverlicense</asp:ListItem>
                                                <asp:ListItem Value="ShopLicense">ShopLicense</asp:ListItem>
                                                <asp:ListItem Value="Others">Others</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldAddressProof5" Display="None" InitialValue="--Select--" ControlToValidate="ddlAddressProof" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select Address Proof "></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 col-xm-12" runat="server" style="display: none;" id="divAddressProof">
                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                            <asp:FileUpload ID="flgUplodMyAddressProof" CssClass="btn btn-small btn-default form-control" onchange="onAddfileupload();" runat="server" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                                            <br />
                                            <asp:Label ID="lbl_emsg1" runat="server" ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lbl_smsg1" runat="server" ForeColor="Green"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="None" ControlToValidate="flgUplodMyAddressProof" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select Address Proof document"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-z select-grid-gap">
                                        <div class="col1">
                                            <label for="exampleInputEmail1" style="width: 200px">Signature Proof.<span class="err">*</span></label>
                                            <asp:DropDownList runat="server" CssClass="maximus-select w-100" ID="ddlSignature" onchange="onClientSignatureSelectedIndexChanged()">
                                                <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                                <asp:ListItem Value="Pancard">Pancard</asp:ListItem>
                                                <asp:ListItem Value="VoterId">VoterId</asp:ListItem>
                                                <asp:ListItem Value="Passport">Passport</asp:ListItem>
                                                <asp:ListItem Value="BankAttested">BankAttested</asp:ListItem>
                                                <asp:ListItem Value="Driverlicense">Driverlicense</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldSignature" Display="None" InitialValue="--Select--" ControlToValidate="ddlSignature" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select Signature Proof"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6 col-xm-12" runat="server" style="display: none;" id="divSigProof">
                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                            <asp:FileUpload ID="flgUplodMySignatureProof" CssClass="btn btn-small btn-default form-control" onchange="onSigfileupload();" runat="server" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                                            <br />
                                            <asp:Label ID="lbl_emsg2" runat="server" ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lbl_smsg2" runat="server" ForeColor="Green"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="None" ControlToValidate="flgUplodMySignatureProof" SetFocusOnError="true" ValidationGroup="bcregs" runat="server" CssClass="err" ErrorMessage="Please select Signature Proof document"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <%--<div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <asp:CheckBox ID="chkValidaddress" runat="server" />

                                        <label for="exampleInputEmail1">Confirmation on Visit on the Local Address and the address is valid</label>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <asp:CheckBox ID="chkvaliddocs" runat="server" />
                                        <label for="exampleInputEmail1">Upload Documents are Verified </label>
                                    </div>--%>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                        <b>
                                            <label for="exampleInputEmail1">Note: </label>
                                        </b>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                        <b>
                                            <label for="exampleInputEmail1">1.Pdf should be less than 2 MB. </label>
                                        </b>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                        <b>
                                            <label for="exampleInputEmail1">2.JPEG,JPG should be less than 2 MB. </label>
                                        </b>
                                    </div>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                        <%--ValidationGroup="bcregs"  OnClick="BtnSubmit_Click" OnClientClick="disableMultipleClick();return checkAgreement();" --%><%--Confirm1();--%>
                                        <asp:Button runat="server" ID="BtnSubmit" ValidationGroup="bcregs" Class="themeBtn themeApplyBtn" OnClick="BtnSubmit_Click" Text="Submit" OnClientClick="Confirm1();"></asp:Button>
                                        <%--OnClick="BtnSubmit_Click"--%>
                                        <%--OnClick="btnSubmitDetails_Click"--%>
                                        <asp:Button ID="BtnBack" runat="server" OnClick="BtnBack_Click" Text="Back" class="themeBtn resetBtn themeCancelBtn me-0" />

                                        <%--<button type="submit" id="" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" onserverclick="BtnBack_ServerClick">Back</button>--%>
                                        <%--onserverclick="btnCancel_ServerClick"--%>
                                        <asp:ValidationSummary
                                            HeaderText="You must enter or select a value in the following fields:"
                                            DisplayMode="BulletList"
                                            EnableClientScript="true"
                                            CssClass="err"
                                            ShowMessageBox="true"
                                            ShowSummary="false"
                                            ForeColor="Red"
                                            ValidationGroup="bcregs"
                                            runat="server" />

                                    </div>

                                    <asp:Button ID="btntest" runat="server" CausesValidation="false" class="btn btn-danger" Text="TestModal" Visible="false"></asp:Button><%-- OnClick="btntest_Click" --%>
                                    <asp:Button ID="btnEditREquest" runat="server" Visible="true" Text="sagfsdf" Style="visibility: hidden" />

                                    <Ajax:ModalPopupExtender ID="ModalPopupExtenderEditRequest" runat="server"
                                        TargetControlID="btnEditREquest" PopupControlID="PanelEditRequest"
                                        PopupDragHandleControlID="PopupHeader_PanelEditRequest" Drag="true" BehaviorID="mpe3"
                                        BackgroundCssClass="modalBackground">
                                    </Ajax:ModalPopupExtender>


                                    <asp:Panel ID="PanelEditRequest" Style="display: none;" runat="server">
                                        <div class="modal-dialog modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h4 class="modal-title">BC Details</h4>
                                                </div>
                                                <div class="modal-body">
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--</asp:Panel>--%>
                </div>

                <div id="DivBcDetails" runat="server" visible="false">
                    <div class="accordion summary-accordion" id="history-accordionvs">
                        <div class="accordion-item">
                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummarythree">
                                <h6 class="searchHeader-heading">BC Details</h6>
                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummarythree"
                                    aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-hide"></span>
                                    <p>Show / Hide</p>
                                </button>
                            </div>

                            <div id="collapseSummarythree" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                data-bs-parent="#summary-accordion">
                                <div class="accordion-body">
                                    <hr class="hr-line">
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                        <!-- input -->
                                        <div class="col" style="display: none">
                                            <label for="exampleInputEmail1">Client<span class="err">*</span></label>
                                            <div class="selectInputDateBox w-100">
                                                <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlcl" PlaceHolder="Client" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col" id="Div8" runat="server" visible="false" style="width: 800px">
                                            <asp:Label ID="Label1" runat="server" Text="Services Offer "></asp:Label><span class="err">*</span><br />

                                            <asp:CheckBox ID="CheckAEPS" runat="server" Style="font-weight: 500" />
                                            <asp:Label ID="Label3" runat="server">AEPS &emsp;</asp:Label>

                                            <asp:CheckBox ID="CheckMATM" runat="server" />
                                            <asp:Label ID="Label4" runat="server">Micro ATM &emsp;</asp:Label>
                                        </div>

                                    </div>
                                    <br />

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <div class="col1">
                                            <label for="exampleInputEmail1">BC Name <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtbcname" TextMode="MultiLine" ReadOnly="true" PlaceHolder="First Name" Width="100%" MaxLength="50"></asp:TextBox>
                                        </div>

                                        <div class="col">
                                            <label for="exampleInputEmail1">Gender <span class="err"></span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="DDlgen" ReadOnly="true" PlaceHolder="Gender" Width="100%" MaxLength="50"></asp:TextBox>
                                            <%--   <asp:DropDownList runat="server" class="form-control" ID="DDlgen" Enabled="false" Width="100%">
                                                    <asp:ListItem Value="Male">Male</asp:ListItem>
                                                    <asp:ListItem Value="Female">Female</asp:ListItem>
                                                    <asp:ListItem Value="Transgender">Transgender</asp:ListItem>
                                                </asp:DropDownList>--%>
                                        </div>

                                        <div class="col" id="Div9" runat="server" style="display: normal;">
                                            <label for="exampleInputEmail1">PAN No. <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtpan" PlaceHolder="Enter 10 digit PAN No." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">GST No. <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtgst" MaxLength="15" Width="100%" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                        <div class="col">
                                            <label for="exampleInputEmail1">Aadhaar card No. <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtaadh" Width="100%" ReadOnly="true" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">BC Category <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="DDLcat" PlaceHolder="BC Category" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">Type Of Organization <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="DDlOrgn" PlaceHolder="Type Of Organization" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                        </div>

                                        <div class="col">
                                            <label for="exampleInputAccountNumber">Account Number <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtacc" Width="100%" ReadOnly="true" PlaceHolder="Account Number" MaxLength="16"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputIFSCode">IFSC Code<span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtifsc" PlaceHolder="IFSC Code" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                    </div>
                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                        <div class="col1">
                                            <label for="exampleInputEmail1">Registered Address <span class="err">*</span></label>
                                            <asp:TextBox runat="server" class="form-control" ID="txtadd" TextMode="MultiLine" ReadOnly="true" Width="100%" PlaceHolder="Registered Address " Style="resize: none"></asp:TextBox>
                                        </div>
                                        <!-- input -->
                                        <div class="col">
                                            <label for="exampleInputEmail1">Country <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlcountrys" PlaceHolder="Country" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">State <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlstates" PlaceHolder="State" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">District  <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddldist" Width="100%" ReadOnly="true" PlaceHolder="Enter district" MaxLength="25">
                                            </asp:TextBox>
                                        </div>
                                        <br />
                                    </div>

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                        <div class="col">
                                            <label for="exampleInputEmail1">City <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlcitys" PlaceHolder="City" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                        </div>

                                        <div class="col">
                                            <label for="exampleInputEmail1">Pincode <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ReadOnly="true" Width="100%" ID="txtpin" MaxLength="6"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">Personal Email Id <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtemail" PlaceHolder="Email ID" ReadOnly="true" Width="100%" MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">Contact No. <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtcontact" PlaceHolder="Enter contact No." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label for="exampleInputEmail1">Landline No. <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtlandline" PlaceHolder="Enter landline no." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div style="height: 20px"></div>


                                    <hr class="hr-line">
                                    <center>
                                        <div class="whos-speaking-area speakers pad100">
                                            <div class="container">
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <div class="section-title text-center">
                                                            <div style="margin-top: 25px; margin-bottom: 35px;">
                                                                <h2>Documents</h2>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- /col end-->
                                                </div>
                                                <!-- /.row  end-->
                                                <div class="row mb50">
                                                    <!-- /col end-->
                                                    <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                        <div class="speakers xs-mb30" style="width: 132px; height: 108px;">
                                                            <%--width: 196px;--%>
                                                            <div class="spk-img">
                                                                <center>
                                                                    <%--<img class="img-fluid" id="img1" src="<%=pathId%>" alt="trainer-img" style="height: 86px;width: 128px;" /></center>--%>
                                                                <ul>
                                                                    <li>
                                                                        <asp:ImageButton ID="btnViewDownloadID" ImageUrl="../../images/download.png"  runat="server" Width="36px" Height="36px" Style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="DownloadDocOne_Click" />
                                                                    </li>
                                                                    <li>
                                                                         <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('IdProof');"></i>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                            <div class="spk-info" style="height: 17px;">  <%--margin-top: -7px;--%>
                                                                <h3>Identity Proof</h3>
                                                                <%-- <p>Address Proof</p>--%>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                        <div class="speakers xs-mb30"  style="width: 132px; height: 108px;">
                                                            <div class="spk-img">
                                                                <center>
                                                                         <%--<img class="img-fluid" src="<%=PathAdd%>" alt="trainer-img"  style="height: 86px;width: 128px;" /></center>--%>
                                                                <ul>
                                                                    <li>
                                                                         <asp:ImageButton ID="btnViewDownloadAdd"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="DownloadDocTwo_Click" />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                    </li>
                                                                    <li>
                                                                         <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('AddProof');"></i>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                            <div class="spk-info" style="height: 17px;">
                                                                <h3>Address Proof</h3>
                                                                <%--<p>Address Proof</p>--%>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                        <div class="speakers xs-mb30"  style="width: 132px; height: 108px;">
                                                            <div class="spk-img">
                                                                <center>
                                                                          <%--<img class="img-fluid" src="<%=PathSig%>" alt="trainer-img" style="height: 86px;width: 128px;"  />--%></center>
                                                                <ul>
                                                                    <li>
                                                                         <asp:ImageButton ID="btnViewDownloadSig"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="DownloadDocThree_Click"/>
                                                                    </li>
                                                                    <li>
                                                                        <i><img  src="../../images/eyeview.png" style="width:36px; height:36px; margin-left: -47px; margin-top: -31px; font-size:35px;" onclick="SetValue('SigProof');"></i>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                            <div class="spk-info" style="height: 17px;">
                                                                <h3>Signature Proof</h3>
                                                                <%--<p>Address Proof</p>--%>
                                                            </div>
                                                        </div>

                                                    </div>

                                                </div>
                                                <!-- /row end-->

                                            </div>
                                            <!-- /container end-->
                                        </div>
                                        </center>

                                    <div class="col-sm-77">
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                            <div class="col-md-12 col-xm-12">
                                                <asp:CheckBox ID="ChkConfirmBC" runat="server" />
                                                <label for="exampleInputEmail1">Confirmation on all above BC Deatils are properly filled</label>
                                            </div>
                                            </b>
                                        </div>
                                    </div>

                                    <hr class="hr-line">

                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                        <%--<button type="button" id="downloadPass" runat="server" visible="true" OnClick="" class="btn btn-success">Submit</button>--%>
                                        <asp:Button runat="server" ID="downloadPass" CssClass="themeBtn themeApplyBtn" OnClick="ProcessBCData_Click" ToolTip="Submit" Text="Submit" OnClientClick="Confirm3();"></asp:Button>
                                        <asp:Button runat="server" ID="btnCloseReceipt" CssClass="themeBtn resetBtn themeCancelBtn me-0r" OnClick="btnCloseReceipt_Click" OnClientClick="Confirm();" ToolTip="Close" Text="Back"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="HidBCID" runat="server" />
                <asp:HiddenField ID="hidimgId" runat="server" />
                <asp:HiddenField ID="hidimgAdd" runat="server" />
                <asp:HiddenField ID="hidimgSig" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnSubmitDetails" EventName="click" />
                <asp:PostBackTrigger ControlID="BtnSubmit" />
                <asp:PostBackTrigger ControlID="btnViewDownloadId" />
                <asp:PostBackTrigger ControlID="btnViewDownloadAdd" />
                <asp:PostBackTrigger ControlID="btnViewDownloadSig" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                <asp:PostBackTrigger ControlID="btnExportCSV" />
                <asp:PostBackTrigger ControlID="btndownload" />
            </Triggers>
        </asp:UpdatePanel>
    </div>




    <br />

    <div class="row backImg" id="divlogoupdate" style="display: none">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Update Logo                            
                </div>
                <div class="clearfix"></div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col">
                        </div>
                        <div class="col">
                        </div>
                        <div class="col">
                        </div>
                        <div class="col" style="text-align: -webkit-right">
                            <label for="exampleInputEmail1">Logo <span class="err">*</span></label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" Display="None" ValidationGroup="Logo" CssClass="err" ControlToValidate="filelogo" ErrorMessage="Please select your logo">
                            </asp:RequiredFieldValidator>
                            <asp:Image ID="imageupload" runat="server" Height="110px" Width="160px" BorderStyle="Double" ImageUrl="../../../images/Profile1.png" />
                            <asp:FileUpload ID="filelogo" Width="160px" runat="server" AllowedFileTypes="jpg,jpeg,png,gif" Style="margin-top: 5px" onchange="return FunImageUpdateLogo(this); " CssClass="btn btn-small btn-default" AllowMultiple="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix"></div>
        <hr />
        <div class="col-md-4 col-xm-10 col-md-offset-10">
            <div class="form-group">
                <asp:Button runat="server" ID="btnupload" ValidationGroup="Logo" Class="btn btn-success" Text="Upload"></asp:Button>
                <%--OnClick="btnupload_Click"--%>
                <asp:Button ID="Button2" runat="server" CausesValidation="false" class="btn btn-danger" Text="Cancel"></asp:Button>
                <%--OnClick="btnCancel_ServerClick"--%>
                <asp:ValidationSummary
                    HeaderText="You must enter or select a value in the following fields:"
                    DisplayMode="BulletList"
                    EnableClientScript="true"
                    CssClass="err"
                    ShowMessageBox="true"
                    ShowSummary="false"
                    ForeColor="Red"
                    ValidationGroup="Logo"
                    runat="server" />
            </div>
        </div>
    </div>


    </div>
    </div>


    <%--------------------------------BULK Insert ------------------------%>
    <div class="modal fade modal-primary" id="popBulkFranchiseInsert" aria-hidden="true"
        aria-labelledby="exampleModalPrimary" role="dialog" tabindex="-1">
        <div class="modal-dialog" style="width: 50%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                    <h4 class="modal-title">Bulk Franchise Insert&nbsp;</h4>
                </div>
                <div class="modal-body">
                    <div class="row " id="divsetWallet">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col">
                                    <div class="control-group row">
                                        <div class="controls col-sm-12">
                                            <asp:FileUpload ID="ExcelFileuploadInsert" type="file" class="form-control" runat="server" Width="164px" Style="margin-left: 336px" />
                                            <asp:TextBox ID="txtbulkfilepathInsert" runat="server" Style="display: none"></asp:TextBox>
                                            <br />
                                            <asp:Button ID="btnUploadBulkTask" runat="server" Text="Upload" class="saveBtn btn btn-primary pull-right" />
                                            <%--OnClick="btnUploadBulkTask_Click"--%>
                                            <%--OnClientClick="fntab1ValicationOnddlDataStatusInsert(event,this);return false;"></asp:Button>--%>
                                        </div>
                                        <div class="controls col-sm-12" style="margin-top: 13px">
                                            <div>
                                                <asp:ImageButton ID="downsamplefrinsert" runat="server" ImageUrl="~/images/download.png" Width="60px" Height="50px" Style="margin-top: -76px"></asp:ImageButton>
                                                <%--OnClick="downsamplefrinsert_Click"--%>
                                            </div>
                                            <div style="width: 138px; float: left">
                                                <asp:Label ID="Label2" runat="server" CssClass="control-label col-xs-12 iceLabel" Text="Download Sample "></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="control-group row">
                                        <div class="controls col-sm-12" style="width: 300px; float: left;">
                                            <div style="width: 138px; float: left">
                                            </div>
                                        </div>
                                    </div>
                                    <asp:FileUpload ID="FileUpload2" runat="server" Width="95px" Style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--<script>
         $(function () {
             $("#<%=ddlState.ClientID%>").select2();
                            $("#<%=ddlState.ClientID%>").select2();
                            $("#<%=ddlCity.ClientID%>").select2();
                         
                        })
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        if (prm != null) {
                            prm.add_endRequest(function (sender, e) {
                                if (sender._postBackSettings.panelsToUpdate != null) {
                                    $("#<%=ddlState.ClientID%>").select2();
                               
                                    $("#<%=ddlCity.ClientID%>").select2();
                                  
                                }
                            });
                        };
                    </script>--%>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#<%=ddlbcCode.ClientID%>").select2();
                    $("#<%=ddlGender.ClientID%>").select2();
                    $("#<%=ddlState.ClientID%>").select2();
                    $("#<%=ddlCity.ClientID%>").select2();
                    $("#<%=ddlCategory.ClientID%>").select2();
                    $("#<%=DDlOrg.ClientID%>").select2();
                    $("#<%=ddlCountry.ClientID%>").select2();
                    $("#<%=ddlAddressProof.ClientID%>").select2();
                    $("#<%=ddlIdentityProof.ClientID%>").select2();
                    $("#<%=ddlSignature.ClientID%>").select2();
                    $("#<%=ddleducation.ClientID%>").select2();
                    $("#<%=ddlRequestStatus.ClientID%>").select2();
                    $("#<%=ddlRequestType.ClientID%>").select2();
                    $("#<%=ddlDistrict.ClientID%>").select2();
                }
            });
        };
    </script>
</asp:Content>
