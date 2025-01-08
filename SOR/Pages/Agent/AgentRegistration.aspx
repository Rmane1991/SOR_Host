<%@ Page Title="" Language="C#" MasterPageFile="~/SOR.Master" AutoEventWireup="true" CodeBehind="AgentRegistration.aspx.cs" Inherits="SOR.Pages.Agent.AgentRegistration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">

    <%--<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>--%>
    <script src="../../js/jquery-3.5.1.js"></script>
    <%--<script type="text/javascript" src="../../JavaScripts/FranchiseManagement.js"></script>--%>

    <script src="../../Scripts/aes.js"></script>
    <script src="../../Scripts/jquery-1.4.1-vsdoc.js"></script>
    <script src="../../Scripts/executeSec.js"></script>
    <script src="../../assets1/multi/jquery.min.js"></script>
    <style>
        span {
            font-size: 12px;
        }

        .selectGrid-m-z {
            /* margin-top: 8px; */
            margin-bottom: 1px;
        }

        .err {
            color: #fe1a03;
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
    <script type="text/javascript"> 
        function onIdFileupload() {
            debugger;
            document.getElementById("<%=lbl_smsg.ClientID%>").innerText = "";
            document.getElementById("<%=lbl_emsg.ClientID%>").innerText = "";
            $(function () {
                var fileUpload = $('#<%=flgUplodMyIdProof.ClientID%>').get(0);

                var files = fileUpload.files;
                var test = new FormData();
                for (var i = 0; i < files.length; i++) {
                    test.append(files[i].name, files[i]);
                }
                $.ajax({
                    url: "Handler5.ashx",
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: test,
                    success: function (result) {
                        document.getElementById("<%=lbl_smsg.ClientID%>").innerText = "";
                        document.getElementById("<%=lbl_emsg.ClientID%>").innerText = "";

                        debugger;
                        if (result.split(':')[0] == "File Uploaded Successfully") {
                            document.getElementById("<%=lbl_smsg.ClientID%>").innerText = result.split(':')[0];
                        } else {
                            document.getElementById("<%=lbl_emsg.ClientID%>").innerText = result.split(':')[0];
                            document.getElementById("<%=flgUplodMyIdProof.ClientID%>").value = "";
                        }
                    },
                    error: function (err) {
                        alert('wrong');
                        alert(err.statusText);
                    }
                });
            })
        }


    </script>

    <%--    <script>
        function checkAgreement() {
            debugger;
            chkValidaddress = document.getElementById('<%= chkValidaddress.ClientID %>');
            chkvaliddocs = document.getElementById('<%= chkvaliddocs.ClientID %>');
            if (chkValidaddress.checked && chkvaliddocs.checked) {

            }
            else {
                alert('Please check confirmation for address and documents,.Declaration')

            }
        }
    </script>--%>

    <script>

        function CheckValidation() {

            var fuData = document.getElementById('<%= UploadForm.ClientID %>');
            var fuData = document.getElementById('<%= flgUplodMyIdProof.ClientID %>');
            var fuData = document.getElementById('<%= flgUplodMyAddressProof.ClientID %>');
            var FileUploadPath = fuData.value;
            //var FileUploadPath = fuData1.value;
            //var FileUploadPath = fuData2.value;

            var file = FileUploadPath.split('\\')[2];



            if (FileUploadPath == '') {
                alert('Please select file to upload');
                return false;
            }


            return true;



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
                    url: "Handler4.ashx",
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
                    url: "Handler6.ashx",
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
        function SetValue(myval) {


            window.open("/pdfhandler.aspx?value=" + myval + "");
        }
    </script>

    <script type="text/javascript">
        var updateProgress = null;

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
                document.getElementById("<%= hdnUserConfirmation.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= hdnUserConfirmation.ClientID %>").value = "No";
            }




            //document.forms[0].appendChild(confirm_value);
        }
        function Confirm1() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Upload Your Data?")) {
                debugger;
                document.getElementById("<%= hdnUploadConfirmation.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= hdnUploadConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
        }
        function Confirm3() {
            let confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want To Save Your Data?")) {
                debugger;
                document.getElementById("<%= hdnFinalConfirmation.ClientID %>").value = "Yes";
            }
            else {
                document.getElementById("<%= hdnFinalConfirmation.ClientID %>").value = "No";
            }

            //document.forms[0].appendChild(confirm_value);
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

            document.getElementById("<%=txtEmailID.ClientID %>").value = '';
            document.getElementById("<%=txtPinCode.ClientID %>").value = '';


            document.getElementById("<%=txtRegisteredAddress.ClientID %>").value = '';

            document.getElementById("<%=txtContactNo.ClientID %>").value = '';
            document.getElementById("<%=txtLandlineNo.ClientID %>").value = '';


            <%--document.getElementById("<%=txtDistrict.ClientID %>").value = '';
            document.getElementById("<%=txtshopDistrict.ClientID %>").value = '';--%>


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
            document.getElementById("<%=flgUplodMyIdProof.ClientID%>").value = "";
            document.getElementById("<%=lbl_smsg.ClientID%>").innerText = "";
            document.getElementById("<%=lbl_emsg.ClientID%>").innerText = "";
            if (ddlRegisteredtype == "--Select--") {
                $("#<%=divIdProof.ClientID %>").hide();
                $("#<%=dvfield_PANNos.ClientID %>").hide();
                document.getElementById("<%=ddlSignature.ClientID %>").disabled = false;
                document.getElementById("<%=ddlAddressProof.ClientID %>").disabled = false;

                document.getElementById("<%=RequiredFieldValidator1.ClientID %>").enabled = true;

                document.getElementById('<%=CheckBoxAddress.ClientID%>').checked = true;
                alert('Please select Identity Proof.');
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
            document.getElementById("<%=flgUplodMySignatureProof.ClientID%>").value = "";
            document.getElementById("<%=lbl_smsg2.ClientID%>").innerText = "";
            document.getElementById("<%=lbl_emsg2.ClientID%>").innerText = "";
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
            document.getElementById("<%=flgUplodMyAddressProof.ClientID%>").value = "";
            document.getElementById("<%=lbl_smsg1.ClientID%>").innerText = "";
            document.getElementById("<%=lbl_emsg1.ClientID%>").innerText = "";
            if (ddlRegisteredtype == "--Select--") {
                $("#<%=divAddressProof.ClientID %>").hide();
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
    <style type="text/css">
        .GridView {
            width: 100%;
            height: auto;
            overflow-y: auto;
            border: solid 0px #B9B9B9 !important;
        }

            .GridView table {
                width: 1%;
                border-collapse: collapse;
            }

            .GridView th {
                background: #fbd2ce;
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

            .GridView tr:nth-child(even) {
                background: #eaecff !important;
            }

            .GridView tr:nth-child(odd) {
                background: #FFF !important;
            }
    </style>
    <style>
        .GridView {
            width: 100% !Important;
        }

        .form-group {
            margin-bottom: 5px;
        }

        table {
            width: 0% !Important;
        }

        .tabs {
            font-family: Calibri;
            font-size: 9pt;
        }
        /*table {
            width: 100% !Important;
        }*/

        fieldset.scheduler-border {
            border: solid 1px lightgray !important;
            border-radius: 5px;
            padding: 0 0px 10px 5px;
            font-size: small;
            border-bottom: none;
            overflow-y: hidden;
            overflow-x: hidden;
        }

        .select2 {
            width: 100% !important;
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

        .ajax__tab_xp .ajax__tab_body {
            font-family: inherit;
        }

        .ajax__tab_xp .ajax__tab_tab {
            height: 19px;
            padding: 4px;
            margin: 0;
        }

        span {
            font-family: var(--primary-font);
            font-size: small;
        }

        .nav-tabs-wrapper .nav-tabs .nav-link {
            background-color: var(--border-box-bg);
            border: 1px solid var(--color-border-default);
            border-radius: 8px 8px 0px 0px;
            font-weight: 500;
            font-size: small;
            color: var(--color-black);
            padding: 9px 12px;
        }

        a {
            color: #000000;
        }
    </style>
    <style>
        .nav-tabs-wrapper .nav-tabs .nav-link.active {
            background-color: #fbd2ce;
            font-weight: 600;
            color: var(--color-primary-default);
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

    <script type="text/javascript">

        function CloseModal() {
            debugger;
            $('#ModalBC').modal('hide');
        }

        function openModal() {
            debugger;
            $('#ModalBC').modal('show');
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

        if (document.getElementById("<%=txtPass.ClientID %>") != "" && document.getElementById("<%=txtPass.ClientID %>") != null) {
            var result = "";
            var lblValueU = document.getElementById("<%=txtPass.ClientID %>");
            var newpwdU = document.getElementById("<%=txtPass.ClientID %>");
            newpwdU.value = fetchAscii(lblValueU.value);
            var Data = document.getElementById("<%=hdtxtpassportno.ClientID %>");
            Data.value = newpwdU.value;
            result = Encry(document.getElementById("<%=hdtxtpassportno.ClientID %>").value);
            document.getElementById("<%=hdtxtpassportno.ClientID %>").value = result;
            document.getElementById("<%=txtPass.ClientID %>").value = '';
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

    <script type="text/javascript">
        function ValidateDob() {
            debugger;
            var UserDate = document.getElementById("<%= txtdob.ClientID %>").value;
            var ToDate = new Date();
            if (new Date(UserDate).getTime() >= ToDate.getTime()) {
                alert("You can not select date greater than today.");
                document.getElementById("<%= txtdob.ClientID %>").value = ToDate.getTime();
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function SetactiveTab() {
            debugger;
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "AGReg";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        };


        //function SetTab() {
        //    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "AGReg";
        //    $('#Tabs a[href="#' + tabName + '"]').tab('show');
        //    $("#Tabs a").click(function () {
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //};



        function SetTab() {
            debugger;
            $('button[data-bs-target="#BulkAgentReg"]').tab('show');
        }

        function ExpandCollapse(obj) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div) {
                if (div.style.display == "none") {
                    div.style.display = "block";
                    img.src = "../../Images/minus.png";
                }
                else {
                    div.style.display = "none";
                    img.src = "../../Images/plus.png";
                }
            }
        }
        function fn_Test() {
            alert();
            return true;
        }
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphmastermain" runat="server">
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:UpdateProgress ID="upContentBodyUpdateProgress" runat="server" AssociatedUpdatePanelID="upContentBody">
            <ProgressTemplate>
                <div style="width: 100%; height: 100%; opacity: 0.8; background-color: black; position: fixed; top: 0; left: 0">
                    <img alt="" id="progressImage1" style="margin-top: 20%" src='<%=Page.ResolveClientUrl("../../Images/loading2_1.gif") %>' />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>

    <asp:Panel ID="upPanel" runat="server" HorizontalAlign="Center" Width="100%">
    </asp:Panel>
    <div style="clear: both;" />
    <asp:HiddenField ID="hidPan" runat="server" />
    <asp:HiddenField ID="hidAadh" runat="server" />
    <asp:HiddenField ID="hdAgentId" runat="server" />
    <asp:HiddenField ID="hdTerminalId" runat="server" />
    <asp:HiddenField ID="hidSgst" runat="server" />
    <asp:HiddenField ID="hidAccIFC" runat="server" />
    <asp:HiddenField ID="hidAccNo" runat="server" />
    <asp:HiddenField ID="hdnUserConfirmation" runat="server" />
    <asp:HiddenField ID="hdnDeleteConfirmation" runat="server" />
    <asp:HiddenField ID="hdnUploadConfirmation" runat="server" />
    <asp:HiddenField ID="hdnFinalConfirmation" runat="server" />
    <asp:HiddenField ID="hidTAB" runat="server" Value="#home" />
    <asp:HiddenField ID="AddPage" Value="#Upload" runat="server" />
    <div class="container-fluid">
        <div class="breadHeader">
            <h5 class="page-title">Agent Registration</h5>
        </div>
        <asp:UpdatePanel runat="server" ID="upContentBody">
            <ContentTemplate>
                <asp:HiddenField ID="TabName" runat="server" />
                <div class="nav-tabs-wrapper">
                    <ul class="nav nav-tabs">
                        <li class="nav-item">
                            <button class="nav-link active" id="docket-tab" data-bs-toggle="tab" data-bs-target="#AGReg" type="button" role="tab" aria-controls="docket-tab-pane" aria-selected="true">Single</button>
                        </li>
                        <li class="nav-item">
                            <button class="nav-link" id="escalation-tab" data-bs-toggle="tab" data-bs-target="#BulkAgentReg" type="button" role="tab" aria-controls="escalation-tab-pane" aria-selected="false" tabindex="-1">Bulk</button>
                        </li>
                    </ul>

                    <%-- <div class="tab-content border mb-3">--%>
                    <div class="tab-content">
                        <div id="AGReg" class="tab-pane fade show active" tabindex="0">
                            <div id="divAction" runat="server">

                                <div class="accordion summary-accordion" id="history-accordionss">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOne1">
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
                                                    <asp:Button ID="btnAddnew" runat="server" Text="Add New" CssClass="themeBtn themeApplyBtn" OnClick="btnAddnew_Click" />
                                                </span>



                                                <hr class="hr-line">
                                                <!-- grid -->
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Request Type</label>
                                                        <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Registration" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Activation" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="De-activation" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Termination" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="ReEdit" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="ReProcess" Value="5"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Bucket</label>
                                                        <asp:DropDownList ID="ddlBucketId" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Pre-Verification" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Level -1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Level -2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Level -3" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Request Status</label>
                                                        <asp:DropDownList ID="ddlRequestStatus" runat="server" CssClass="maximus-select w-100" AutoPostBack="false">
                                                            <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Declined" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <%--<div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Agent Code</label>
                                                        <div class="inputBox w-100">
                                                            <input type="text" id="txtAgentCode" runat="server" class="input-text form-control" style="width: 100%" placeholder="Agent Code" />
                                                        </div>
                                                    </div>--%>
                                                    <%--<div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Pan No.</label>
                                                        <div class="inputBox w-100">
                                                            
                                                        </div>
                                                    </div>--%>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">Pan No</label>
                                                        <div class="inputBox w-100">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="txtPanNoF" Style="display: none" runat="server" CssClass="err" ValidationGroup="FranchiseReg" ErrorMessage="Please enter pan no"></asp:RequiredFieldValidator>
                                                            <%--<input type="text" id="txtPanNoF" runat="server" class="input-text form-control" style="width: 100%" placeholder="Pan No MaxLength="10" oninput="this.value = this.value.toUpperCase();"/>--%>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPanNoF" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" oninput="this.value = this.value.toUpperCase();"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="1" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPanNoF" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="UppercaseLetters,Numbers, LowercaseLetters" TargetControlID="txtPanNoF" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-center align-items-center">
                                                    <!-- Container for buttons, centered with export buttons on the right -->
                                                    <div class="d-flex justify-content-between align-items-center w-100">
                                                        <!-- Search and Clear buttons (centered) -->
                                                        <div class="d-flex justify-content-center gap-3 w-100">
                                                            <button type="button" id="btnSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnSearch_ServerClick">
                                                                Search
                                                            </button>
                                                            <button type="button" id="btnClear" runat="server" onserverclick="btnClear_ServerClick" class="themeBtn resetBtn themeCancelBtn" data-bs-toggle="modal" style="margin-top: 18px;">
                                                                Clear
                                                            </button>
                                                        </div>

                                                        <!-- Export buttons aligned to the right -->
                                                        <div class="d-flex justify-content-end">
                                                            <asp:ImageButton ID="btnExportCSV" runat="server" ImageUrl="../../images/617449.png" CssClass="iconButtonBox"
                                                                ToolTip="Csv" OnClick="btnExportCSV_Click" data-toggle="modal" data-target="#myModal" />
                                                            <asp:ImageButton ID="btndownload" runat="server" ImageUrl="../../images/4726040.png" CssClass="iconButtonBox"
                                                                ToolTip="Xls" OnClick="btndownloadXLS_Click" data-toggle="modal" data-target="#myModal" />
                                                        </div>
                                                    </div>
                                                </div>




                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
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
                                        <div class="tableBorderBox HeaderStyle" style="width: 100%; padding: 10px 10px; overflow: scroll; max-height: 400px;">
                                            <%--OnPageIndexChanging="gvBusinessCorrespondents_PageIndexChanging"--%>
                                            <asp:GridView ID="gvAgentOnboard" runat="server"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                AllowPaging="true"
                                                CssClass="GridView"
                                                Visible="true"
                                                PagerSettings-Mode="NumericFirstLast"
                                                OnPageIndexChanging="gvAgentOnboard_PageIndexChanging"
                                                OnRowDataBound="gvAgentOnboard_RowDataBound"
                                                OnRowCommand="gvAgentOnboard_RowCommand"
                                                DataKeyNames="Bucket,RequestStatus,Request Type,ActivityType,AgentReqId"
                                                PagerSettings-FirstPageText="First Page"
                                                PagerSettings-LastPageText="Last Page">
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
                                                            <%-- CommandArgument='<%#Eval("BC Code")+","+ Eval("numBCID")%>'--%>   <%--OnClick="btnView_Click"--%>
                                                            <asp:ImageButton ID="btnView" runat="server" ImageUrl="../../images/Edit-01-512.png" Width="20px" Height="20px" CommandName="EditDetails"
                                                                ToolTip="Click Here To Edit Data" data-toggle="modal" data-target="#ModalBC" />
                                                            <asp:ImageButton ID="btndelete" runat="server" ImageUrl="../../images/document-delete.png" OnClientClick="Confirm2();" Width="20px" Height="20px" CommandName="DeleteDetails"
                                                                ToolTip="Click Here To Delete Data" data-toggle="modal" data-target="#ModalBC" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--   <asp:BoundField DataField="Sr.No." HeaderText="Sr.No." />--%>
                                                    <asp:BoundField DataField="AgentReqId" HeaderText="Agent Req. ID" Visible="false" />
                                                    <asp:BoundField DataField="BCCode" HeaderText="BC Code" />
                                                    <asp:BoundField DataField="aggregatorcode" HeaderText="Aggregator Code" />
                                                    <%--<asp:BoundField DataField="AgentCode" HeaderText="Code" />--%>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                    <asp:BoundField DataField="AgentAddress" HeaderText="Address" />
                                                    <asp:BoundField DataField="PanNo" HeaderText="Pan No" />
                                                    <asp:BoundField DataField="AadharNo" HeaderText="Aadhar No" />
                                                    <asp:BoundField DataField="Request Type" HeaderText="Request Type" />
                                                    <asp:BoundField DataField="Bucket" HeaderText="Bucket" Visible="false" />
                                                    <asp:BoundField DataField="RequestStatus" HeaderText="Request Status" />
                                                    <asp:BoundField DataField="ActivityType" HeaderText="ActivityType" Visible="false" />



                                                </Columns>
                                                <HeaderStyle BackColor="#8DCCF6" ForeColor="#3D62B6" />
                                            </asp:GridView>
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
                                                                    <div class="col" style="display: none">
                                                                        <label for="exampleInputEmail1">Client<span class="err">*</span></label>
                                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator43" ControlToValidate="ddlclient" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Client."></asp:RequiredFieldValidator>--%>
                                                                        <div class="selectInputDateBox w-100">
                                                                            <asp:DropDownList ID="ddlclient" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="false">
                                                                                <%--OnSelectedIndexChanged="ddlclient_SelectedIndexChanged"--%>
                                                                                <asp:ListItem Value="0">--All--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col">
                                                                        <label class="exampleInputEmail1" for="selectInputLabel">Business Correspondence:<span class="err">*</span></label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ControlToValidate="ddlbcCode" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select BC."></asp:RequiredFieldValidator>
                                                                        <div class="selectInputDateBox w-100">
                                                                            <asp:DropDownList ID="ddlbcCode" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlbcCode_SelectedIndexChanged">
                                                                                <asp:ListItem Value="0">--Business Correspondents--</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                    <!-- input -->
                                                                    <div class="col">
                                                                        <label class="exampleInputEmail1" for="selectInputLabel">Aggregator:<span class="err">*</span></label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="ddlaggregatorCode" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Aggregator."></asp:RequiredFieldValidator>
                                                                        <div class="selectInputDateBox w-100">
                                                                            <asp:DropDownList ID="ddlaggregatorCode" runat="server" CssClass="maximus-select w-100" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlaggregatorCode_SelectedIndexChanged">
                                                                                <asp:ListItem Value="0">--Aggregator--</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                    <!-- input -->


                                                                    <div class="col" id="Div_Chk" runat="server" style="width: 800px">
                                                                        <asp:Label ID="c" runat="server" Text="Services offered "></asp:Label><span class="err">*</span><br />

                                                                        <asp:CheckBox ID="chkAEPS" runat="server" Style="font-weight: 500" />
                                                                        <asp:Label ID="lblchkAEPS" runat="server">AePS &emsp;</asp:Label>

                                                                        <asp:CheckBox ID="chkdmt" runat="server" />
                                                                        <asp:Label ID="lblchkdmt" runat="server">DMT &emsp;</asp:Label>

                                                                        <asp:CheckBox ID="chkMATM" runat="server" />
                                                                        <asp:Label ID="lblchkMATM" runat="server">Micro ATM &emsp;</asp:Label>
                                                                    </div>

                                                                    <div class="col" style="display: none" runat="server" visible="false">
                                                                        <label for="exampleInputEmail1">Franchise Profile Image  <span class="err">*</span></label>
                                                                        <asp:Image ID="imgMyImge" runat="server" Height="110px" Width="110px" BorderStyle="Double" ImageUrl="../../../images/Profile1.png" />
                                                                        <asp:FileUpload ID="flgUplodMyImage" Width="120px" runat="server" Style="margin-top: 5px" onchange="return FunImageFIll(this);" CssClass="btn btn-small btn-default" />
                                                                        <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                                                    </div>
                                                                </div>
                                                                <br />

                                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                                    <div class="col" id="dvfield_PANNo" runat="server" style="display: normal;">
                                                                        <label for="exampleInputEmail1">PAN No. <span class="err">*</span></label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtPANNo" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter pan no"></asp:RequiredFieldValidator>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPANNo" OnTextChanged="txtPANNo_TextChanged" AutoPostBack="true" PlaceHolder="Enter 10 digit PAN No." Width="100%" MaxLength="10" oninput="this.value = this.value.toUpperCase();"></asp:TextBox>
                                                                        <asp:Label ID="lblPanNoError" runat="server" Text="" CssClass="err"></asp:Label>
                                                                        <asp:HiddenField ID="hd_txtPANNo" runat="server" Value="1" />
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationGroup="FranchiseReg" runat="server" Display="None" ErrorMessage="Provided PAN No is not valid! Please enter PAN No start with 'eg.ABCDE1234K'." ControlToValidate="txtPANNo" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}" />
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="UppercaseLetters,Numbers, LowercaseLetters" TargetControlID="txtPANNo" />
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">First Name <span class="err">*</span></label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" ControlToValidate="txtFirstName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFirstName" PlaceHolder="First Name" Width="100%" MaxLength="20"></asp:TextBox>
                                                                        <asp:HiddenField ID="hd_txtFirstName" runat="server" Value="1" />
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtFirstName" />
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">Middle Name <%--<span class="err">*</span>--%></label>
                                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtMiddleName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Middle name"></asp:RequiredFieldValidator>--%>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtMiddleName" PlaceHolder="Middle Name" Width="100%" MaxLength="20"></asp:TextBox>
                                                                        <asp:HiddenField ID="HiddenFieldM" runat="server" Value="1" />
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtMiddleName" />
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">Last Name <%--<span class="err">*</span>--%></label>

                                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtLastName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>--%>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLastName" PlaceHolder="Last Name" Width="100%" MaxLength="20"></asp:TextBox>
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtLastName" />
                                                                    </div>
                                                                    <%--   <div class="col">
                                                            <label for="exampleInputEmail1">Father Name <span class="err">*</span></label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldlValidator2" ControlToValidate="txtFatherName" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Father name"></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtFatherName" PlaceHolder="Father Name" Width="100%" MaxLength="50"></asp:TextBox>
                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" " TargetControlID="txtFatherName" />

                                                        </div>--%>
                                                                    <%--<div class="col">
                                                                        <label for="exampleInputEmail1">Gender  <span class="err"></span></label>
                                                                        <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlGender" Width="100%">
                                                                            <asp:ListItem Value="Male">Male</asp:ListItem>
                                                                            <asp:ListItem Value="Female">Female</asp:ListItem>
                                                                            <asp:ListItem Value="Transgender">Transgender</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>--%>
                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">Gender  <span class="err"></span></label>
                                                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="maximus-select w-100" Width="100%">
                                                                            <asp:ListItem Value="Male">Male</asp:ListItem>
                                                                            <asp:ListItem Value="Female">Female</asp:ListItem>
                                                                            <asp:ListItem Value="Transgender">Transgender</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">GST No.</label>

                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtGSTNo" MaxLength="15" Width="100%" PlaceHolder="Enter 15 digit GST no."></asp:TextBox>
                                                                        <asp:HiddenField ID="hd_txtGSTNo" runat="server" Value="1" />
                                                                    </div>

                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">Aadhaar card No.</label><span class="err">*</span>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtaadharno" Width="100%" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtaadharno" ReadOnly="true" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Aadhaar card no"></asp:RequiredFieldValidator>
                                                                        <asp:HiddenField ID="hd_txtaadharno" runat="server" Value="1" />
                                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator41" ControlToValidate="txtaadharno" Display="None" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Aadhaar Card no"></asp:RequiredFieldValidator>--%>
                                                                    </div>

                                                                    <%-- <div class="col"><label for="exampleInputEmail1">Aadhaar card No. <span class="err">*</span>
                                                                      </label><asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox4" Width="100%" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12" onkeypress="return Numeric(event)"></asp:TextBox>
                                                                        <asp:HiddenField ID="hd_txtaadharno" runat="server" Value="1" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="txtaadharno" ReadOnly="true" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Aadhaar card no"></asp:RequiredFieldValidator>
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers,UppercaseLetters" ValidChars="1234567890X" TargetControlID="txtaadharno" />

                                                                    </div>--%>
                                                                    <div class="col">
                                                                        <label for="exampleInputEmail1">Agent Category</label>
                                                                        <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlCategory" Width="100%">
                                                                            <asp:ListItem Value="General">General</asp:ListItem>
                                                                            <asp:ListItem Value="OBC">OBC</asp:ListItem>
                                                                            <asp:ListItem Value="SC">SC</asp:ListItem>
                                                                            <asp:ListItem Value="ST">ST</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>

                                                                    <div class="col">
                                                                        <label class="selectInputLabel" for="selectInputLabel">Agent DOB <span class="err">*</span> </label>
                                                                        <div class="selectInputDateBox w-100">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtdob" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please Select AgentDOB"></asp:RequiredFieldValidator>
                                                                            <asp:TextBox ID="txtdob" runat="server" Width="100%" CssClass="input-text form-control" TextMode="Date" onchange="javascript:ValidateDob();"
                                                                                oncopy="return false;" oncut="return false;" onkeypress="return block(event);"
                                                                                onpaste="return false;"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col">
                                                                        <label for="exampleInputAccountNumber">Agent Account Number <span class="err">*</span></label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" ControlToValidate="txtAccountNumber" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Account Number"></asp:RequiredFieldValidator>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAccountNumber" Width="100%" PlaceHolder="Account Number" MaxLength="19"></asp:TextBox>
                                                                        <asp:HiddenField ID="hd_txtAccountNumber" runat="server" Value="1" />
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" FilterType="Numbers" TargetControlID="txtAccountNumber" />
                                                                    </div>
                                                                </div>

                                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">IFSC Code<span class="err">*</span></label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator35" ControlToValidate="txtIFsccode" Style="display: none" runat="server" ValidationGroup="FranchiseReg" CssClass="err" ErrorMessage="Please enter IFSC Code"></asp:RequiredFieldValidator>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtIFsccode" PlaceHolder="IFSC Code" Width="100%" MaxLength="11"></asp:TextBox>
                                                                        <asp:HiddenField ID="hd_txtIFsccode" runat="server" Value="1" />
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtIFsccode" />
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">PassPort No</label>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtPass" PlaceHolder="Passport No" Width="100%" MaxLength="11"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdtxtpassportno" runat="server" Value="1" />
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtPass" />
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">Device Code<span class="err">*</span></label>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtcode" PlaceHolder="Device Code" Width="100%" MaxLength="50"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtcode" Style="display: none" runat="server" ValidationGroup="FranchiseReg" CssClass="err" ErrorMessage="Please enter Device Code"></asp:RequiredFieldValidator>
                                                                        <asp:HiddenField ID="hdnUserConfirmation2" runat="server" Value="1" />
                                                                        <%--<Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtcode"  ValidChars="-" ValidateRequestMode="Enabled" />--%>
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">Population Group<span class="err">*</span></label>
                                                                        <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlarea" Width="100%">
                                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                            <asp:ListItem Value="Rural">Rural</asp:ListItem>
                                                                            <asp:ListItem Value="Urban">Urban</asp:ListItem>
                                                                            <asp:ListItem Value="Semi-Urban">Semi-Urban</asp:ListItem>
                                                                            <asp:ListItem Value="Metropolitan">Metropolitan</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="ddlarea" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Population Group."></asp:RequiredFieldValidator>
                                                                    </div>
                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">Terminal Id<span class="err">*</span></label>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtTerminalId" PlaceHolder="Terminal Id" Width="100%" MaxLength="10"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ControlToValidate="txtTerminalId" Style="display: none" runat="server" ValidationGroup="FranchiseReg" CssClass="err" ErrorMessage="Please enter Terminal Id"></asp:RequiredFieldValidator>

                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" FilterType="UppercaseLetters,Numbers" TargetControlID="txtTerminalId" />
                                                                    </div>
                                                                    <%--<div class="col">
                                                                        <label for="exampleInputIFSCode">Agent Code<span class="err">*</span></label>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtagentId" PlaceHolder="Agent Code" Width="100%" MaxLength="15"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="txtagentId" Style="display: none" runat="server" ValidationGroup="FranchiseReg" CssClass="err" ErrorMessage="Please enter Agent Code"></asp:RequiredFieldValidator>

                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers" TargetControlID="txtagentId" />
                                                                    </div>--%>
                                                                </div>
                                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">



                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">Latitude<span class="err">*</span></label>
                                                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtLatitude" Display="None" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please Enter Latitude"></asp:RequiredFieldValidator>--%>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1228" ControlToValidate="txtLatitude" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Latitude"></asp:RequiredFieldValidator>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLatitude" PlaceHolder="Latitude" Width="100%" MaxLength="25"></asp:TextBox>

                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtLatitude" />
                                                                    </div>

                                                                    <div class="col">
                                                                        <label for="exampleInputIFSCode">Longitude <span class="err">*</label>
                                                                        <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtLongitude" PlaceHolder="Longitude" Width="100%" MaxLength="25"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator556" ControlToValidate="txtLongitude" runat="server" Style="display: none" ValidationGroup="FranchiseReg" CssClass="err" ErrorMessage="Please Enter Longitude"></asp:RequiredFieldValidator>
                                                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtLongitude" />
                                                                    </div>
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
                                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                                        <div class="col1">
                                                                            <label for="exampleInputEmail1">Registered Address <span class="err">*</span></label>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtRegisteredAddress" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter registered address"></asp:RequiredFieldValidator>
                                                                            <asp:TextBox runat="server" class="form-control" ID="txtRegisteredAddress" TextMode="MultiLine" Width="100%" PlaceHolder="Registered Address " Style="resize: none" MaxLength="250"></asp:TextBox>
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
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="ddlDistrict" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select district"></asp:RequiredFieldValidator>
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
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="form-group col-md-6">
                                                            <asp:CheckBox ID="CheckBoxagent" runat="server" AutoPostBack="true" onchange="javascript:ShowAddress();" OnCheckedChanged="CheckBoxAddress_CheckedChanged" />
                                                            <label>Shop Details Same As Communication Details</label>
                                                        </div>
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="CheckBoxagent" EventName="CheckedChanged" />

                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <br />


                                        <div style="height: 10px"></div>

                                        <div class="col-md-12">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                </div>
                                                <div class="clearfix"></div>
                                                <div class="panel-body">
                                                    <div class="accordion summary-accordion" id="history-accordionsds">
                                                        <div class="accordion-item" id="divshopcountry" runat="server">
                                                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOnes1">
                                                                <h6 class="searchHeader-heading">Shop Details</h6>
                                                                <button class="show-hide-btn" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSummaryOnes1"
                                                                    aria-expanded="true" aria-controls="collapseOne">
                                                                    <span class="icon-hide"></span>
                                                                    <p>Show / Hide</p>
                                                                </button>
                                                            </div>

                                                            <div id="collapseSummaryOnes1" class="accordion-collapse collapse show" aria-labelledby="headingOne"
                                                                data-bs-parent="#summary-accordion">
                                                                <div class="accordion-body">
                                                                    <hr class="hr-line">
                                                                    <!-- grid -->

                                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                                        <div class="col1">
                                                                            <label for="exampleInputEmail1">Shop Address </label>
                                                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtshopadd" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Shop address"></asp:RequiredFieldValidator>--%>
                                                                            <asp:TextBox runat="server" class="form-control" ID="txtshopadd" TextMode="MultiLine" Width="100%" PlaceHolder="Shop Address " Style="resize: none"></asp:TextBox>
                                                                            <asp:HiddenField ID="HiddenField4" runat="server" Value="1" />
                                                                        </div>
                                                                        <!-- input -->
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop Pincode </label>
                                                                            <asp:TextBox runat="server" CssClass="input-text form-control" PlaceHolder="eg.400601" Width="100%" ID="txtshoppin" AutoPostBack="true" MaxLength="6" OnTextChanged="txtshoppin_TextChanged"></asp:TextBox>
                                                                            <%--  <asp:HiddenField ID="HiddenField9" runat="server" Value="1" />--%>
                                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" TargetControlID="txtshoppin" />
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" ControlToValidate="txtPinCode" ValidationGroup="FranchiseReg" Style="display: none" runat="server" CssClass="err" ErrorMessage="Please enter Shop pincode"></asp:RequiredFieldValidator>--%>
                                                                        </div>
                                                                        <!-- input -->
                                                                        <div class="col" style="display: none">
                                                                            <label for="exampleInputEmail1">Shop Country </label>
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="ddlshopCountry" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Shop country"></asp:RequiredFieldValidator>--%>
                                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlshopCountry" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                                                            <%--OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"--%>
                                                                            <%--<asp:HiddenField ID="HiddenField5" runat="server" Value="1" />--%>
                                                                        </div>
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop State </label>
                                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlShopState" Width="100%" OnSelectedIndexChanged="ddlShopState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                            <%--OnSelectedIndexChanged="ddlState_SelectedIndexChanged"--%>
                                                                            <%--<asp:HiddenField ID="HiddenField6" runat="server" Value="1" />--%>
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddlShopState" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select Shop state"></asp:RequiredFieldValidator>--%>
                                                                        </div>
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop District</label>
                                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" ID="ddlShopDistrict" Width="100%" OnSelectedIndexChanged="ddlShopDistrict_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" ControlToValidate="ddlShopDistrict" Width="100%" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select state"></asp:RequiredFieldValidator>--%>
                                                                        </div>
                                                                        <br />
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop City </label>
                                                                            <asp:DropDownList runat="server" class="form-control" CssClass="maximus-select w-100" Width="100%" ID="ddlShopCity"></asp:DropDownList>
                                                                            <%--  <asp:HiddenField ID="HiddenField8" runat="server" Value="1" />--%>
                                                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ControlToValidate="ddlShopCity" InitialValue="0" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please select city"></asp:RequiredFieldValidator>--%>
                                                                        </div>

                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop EmailId </label>
                                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtshopEmailID" PlaceHolder="Email ID" Width="100%" MaxLength="50"></asp:TextBox>

                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Please enter valid Shop email id" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers,UppercaseLetters, LowercaseLetters, Custom" ValidChars=".@!#$%^&*()_,/\-" TargetControlID="txtEmailID" />
                                                                        </div>
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop Contact No. </label>
                                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox5" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                                            <%-- <asp:HiddenField ID="hdnUserConfirmation0" runat="server" Value="1" />--%>
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" ControlToValidate="TextBox5" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Shop contact no"></asp:RequiredFieldValidator>--%>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '6/7/8/9'." ControlToValidate="txtContactNo" ValidationExpression="^[6789]\d{9}$" />
                                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterType="Numbers" TargetControlID="TextBox5" />
                                                                        </div>
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Shop Landline No. </label>
                                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox6" PlaceHolder="Enter landline no." Width="100%" MaxLength="10"></asp:TextBox>
                                                                            <%--<asp:HiddenField ID="hdnUserConfirmation1" runat="server" Value="1" />--%>
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator22" ControlToValidate="TextBox6" Style="display: none" ValidationGroup="FranchiseReg" runat="server" CssClass="err" ErrorMessage="Please enter Shop landline no"></asp:RequiredFieldValidator>--%>
                                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterType="Numbers" TargetControlID="TextBox6" />
                                                                        </div>
                                                                        <div class="col">
                                                                            <label for="exampleInputEmail1">Alternate No </label>
                                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox9" PlaceHolder="Enter contact No." Width="100%" MaxLength="10"></asp:TextBox>
                                                                            <%--<asp:HiddenField ID="hdnUserConfirmation4" runat="server" Value="0" />--%>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="None" ValidationGroup="FranchiseReg" ErrorMessage="Provided mobile number is not valid! Please enter valid 10 digit mobile number start with '7/8/9'." ControlToValidate="txtAlterNateNo" ValidationExpression="^[789]\d{9}$" />
                                                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" FilterType="Numbers" TargetControlID="TextBox9" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                            <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                                <div>
                                                                    <asp:Button runat="server" ID="btnSubmitDetails" ValidationGroup="FranchiseReg" OnClientClick="Confirm();" OnClick="btnSubmitDetails_Click" Class="themeBtn themeApplyBtn" CommandArgument='<%#Eval("AgentReqId")%>' Text="Submit"></asp:Button>
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
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="None" ControlToValidate="UploadForm" SetFocusOnError="true" ValidationGroup="AgentRegs" runat="server" CssClass="err" ErrorMessage="Please select Registration form"></asp:RequiredFieldValidator>
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
                                                            <asp:RequiredFieldValidator ID="RequiredFieldIdentityProof" Display="None" InitialValue="--Select--" ControlToValidate="ddlIdentityProof" SetFocusOnError="true" ValidationGroup="agentregs" runat="server" CssClass="err" ErrorMessage="Please select IdentityProof "></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6 col-xm-12" runat="server" id="divIdProof" style="display: none;">
                                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                                            <asp:FileUpload ID="flgUplodMyIdProof" CssClass="btn btn-small btn-default form-control" onchange="onIdFileupload();" runat="server" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                                                            <br />
                                                            <asp:Label ID="lbl_emsg" runat="server" ForeColor="Red"></asp:Label>
                                                            <asp:Label ID="lbl_smsg" runat="server" ForeColor="Green"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" Display="None" ControlToValidate="flgUplodMyIdProof" SetFocusOnError="true" ValidationGroup="agentregs" runat="server" CssClass="err" ErrorMessage="Please select IdentityProof document"></asp:RequiredFieldValidator>
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
                                                                <asp:ListItem Value="PhoneBillOnlyMTNLandBSNL">PhoneBillOnly MTNL&BSNL</asp:ListItem>
                                                                <asp:ListItem Value="Driverlicense">Drivinglicense</asp:ListItem>
                                                                <asp:ListItem Value="ShopLicense">ShopLicense</asp:ListItem>
                                                                <asp:ListItem Value="Others">Others</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldAddressProof5" Display="None" InitialValue="--Select--" ControlToValidate="ddlAddressProof" SetFocusOnError="true" ValidationGroup="agentregs" runat="server" CssClass="err" ErrorMessage="Please select Address Proof "></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6 col-xm-12" runat="server" style="display: none;" id="divAddressProof">
                                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                                            <asp:FileUpload ID="flgUplodMyAddressProof" CssClass="btn btn-small btn-default form-control" onchange="onAddfileupload();" runat="server" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                                                            <br />
                                                            <asp:Label ID="lbl_emsg1" runat="server" ForeColor="Red"></asp:Label>
                                                            <asp:Label ID="lbl_smsg1" runat="server" ForeColor="Green"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="None" ControlToValidate="flgUplodMyAddressProof" SetFocusOnError="true" ValidationGroup="agentregs" runat="server" CssClass="err" ErrorMessage="Please select Address Proof document"></asp:RequiredFieldValidator>
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
                                                            <asp:RequiredFieldValidator ID="RequiredFieldSignature" Display="None" InitialValue="--Select--" ControlToValidate="ddlSignature" SetFocusOnError="true" ValidationGroup="agentregs" runat="server" CssClass="err" ErrorMessage="Please select Signature Proof"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6 col-xm-12" runat="server" style="display: none;" id="divSigProof">
                                                            <label for="exampleInputEmail1">File Path. <span class="err">*</span></label>
                                                            <asp:FileUpload ID="flgUplodMySignatureProof" CssClass="btn btn-small btn-default form-control" onchange="onSigfileupload();" runat="server" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                                                            <br />
                                                            <asp:Label ID="lbl_emsg2" runat="server" ForeColor="Red"></asp:Label>
                                                            <asp:Label ID="lbl_smsg2" runat="server" ForeColor="Green"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" Display="None" ControlToValidate="flgUplodMySignatureProof" SetFocusOnError="true" ValidationGroup="agentregs" runat="server" CssClass="err" ErrorMessage="Please select Signature Proof document"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <%-- <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap" visible="false">
                                            <asp:CheckBox ID="chkValidaddress" runat="server"  Visible="false"/>
                                            <label for="exampleInputEmail1">Confirmation on Visit on the Local Address and the address is valid</label>
                                        </div>
                                        <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap" visible="false">
                                            <asp:CheckBox ID="chkvaliddocs" runat="server"  Visible="false"/>
                                            <label for="exampleInputEmail1">Upload Documents are Verified </label>
                                        </div>--%>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                                        <b>
                                                            <label for="exampleInputEmail1">Note: </label>
                                                        </b>
                                                    </div>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                                        <b>
                                                            <label for="exampleInputEmail1">1.JPEG,PNG,Pdf should be less than 2 MB. </label>
                                                        </b>
                                                    </div>
                                                    <%-- <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                            <b>
                                                <label for="exampleInputEmail1">2.JPEG,JPG should be less than 2 MB. </label>
                                            </b>
                                        </div>--%>

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                        <%--ValidationGroup="AgentRegs"  OnClick="BtnSubmit_Click" OnClientClick="disableMultipleClick();return checkAgreement();" --%>
                                                        <asp:Button runat="server" ID="BtnSubmit" Class="themeBtn themeApplyBtn" ValidationGroup="agentregs" OnClick="BtnSubmit_Click" Text="Submit" CommandArgument='<%#Eval("AgentReqId")%>' OnClientClick="Confirm1(); "></asp:Button>
                                                        <%--OnClick="BtnSubmit_Click"--%>
                                                        <%--OnClick="btnSubmitDetails_Click"--%>
                                                        <asp:Button ID="BtnBack" runat="server" OnClick="BtnBack_Click" Text="Back" class="themeBtn resetBtn themeCancelBtn me-0" />
                                                        <%--OnClientClick="return CheckValidation();"--%>
                                                        <asp:ValidationSummary
                                                            HeaderText="You must enter or select a value in the following fields:"
                                                            DisplayMode="BulletList"
                                                            EnableClientScript="true"
                                                            CssClass="err"
                                                            ShowMessageBox="true"
                                                            ShowSummary="false"
                                                            ForeColor="Red"
                                                            ValidationGroup="agentregs"
                                                            runat="server" />

                                                    </div>

                                                    <asp:Button ID="btntest" runat="server" CausesValidation="false" OnClick="btntest_Click" class="btn btn-danger" Text="TestModal" Visible="false"></asp:Button><%-- OnClick="btntest_Click" --%>
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
                                                                    <h4 class="modal-title">Agent Details</h4>
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

                                <div id="DivAgntDetails" runat="server" visible="false">
                                    <div class="accordion summary-accordion" id="history-accordionvs">
                                        <div class="accordion-item">
                                            <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummarythree">
                                                <h6 class="searchHeader-heading">Agent Details</h6>
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
                                                            <label for="exampleInputEmail1">Client</label>
                                                            <div class="selectInputDateBox w-100">
                                                                <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlcl" PlaceHolder="Client" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                                <%--   <asp:DropDownList ID="ddlcl" runat="server" Enabled="false" CssClass="form-control" Width="100%" AutoPostBack="false">
                                                        <asp:ListItem Value="0">--All--</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                            </div>
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Business Correspondence</label>
                                                            <div class="selectInputDateBox w-100">
                                                                <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtbccode" PlaceHolder="BC NAMe" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                                <%--   <asp:DropDownList ID="ddlcl" runat="server" Enabled="false" CssClass="form-control" Width="100%" AutoPostBack="false">
                                                        <asp:ListItem Value="0">--All--</asp:ListItem>
                                                    </asp:DropDownList>--%>
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
                                                            <label for="exampleInputEmail1">Agent Name</label>
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
                                                            <label for="exampleInputEmail1">PAN No.</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtpan" PlaceHolder="Enter 10 digit PAN No." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">GST No.</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtgst" MaxLength="15" Width="100%" ReadOnly="true" PlaceHolder="Enter 15 digit GST no."></asp:TextBox>
                                                        </div>
                                                        <%-- <div class="col">
                                            <label for="exampleInputEmail1">Shop Name. <span class="err">*</span></label>
                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox1" MaxLength="15" Width="100%" ReadOnly="true" PlaceHolder="Enter 15 digit GST no."></asp:TextBox>
                                        </div>--%>
                                                    </div>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Aadhaar card No.</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtaadh" Width="100%" ReadOnly="true" PlaceHolder="Enter 12 digit Aadhaarcard No." MaxLength="12" onkeypress="return isNumber(event)"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Agent Category</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtagentcategory" PlaceHolder="Agent Category" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                            <%--   <asp:DropDownList runat="server" class="form-control" ID="DDLcat" Enabled="false" Width="100%">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>--%>
                                                            <%--</asp:DropDownList>--%>
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputAccountNumber">Account Number</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtacc" Width="100%" ReadOnly="true" PlaceHolder="Account Number" MaxLength="16"></asp:TextBox>

                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputIFSCode">IFSC Code</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtifsc" PlaceHolder="IFSC Code" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Agent DOB</label>

                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtAgrdob" ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    </div>
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                        <div class="col1">
                                                            <label for="exampleInputEmail1">Registered Address</label>
                                                            <asp:TextBox runat="server" class="form-control" ID="txtadd" TextMode="MultiLine" ReadOnly="true" Width="100%" PlaceHolder="Registered Address " Style="resize: none"></asp:TextBox>
                                                        </div>
                                                        <!-- input -->
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Country</label>

                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlcountrys" PlaceHolder="Country" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                            <%--  <asp:DropDownList runat="server" class="form-control" ID="ddlcountrys" Width="100%" Enabled="false" AutoPostBack="true">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>--%>
                                                            <%--OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"--%>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">State</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlstates" PlaceHolder="State" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                            <%-- <asp:DropDownList runat="server" class="form-control" ID="ddlstates" Width="100%" Enabled="false" AutoPostBack="true">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>--%>
                                                            <%--OnSelectedIndexChanged="ddlState_SelectedIndexChanged"--%>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">District</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddldist" Width="100%" ReadOnly="true" PlaceHolder="Enter district" MaxLength="25">
                                                            </asp:TextBox>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">City</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="ddlcitys" PlaceHolder="City" ReadOnly="true" Width="100%" MaxLength="11"></asp:TextBox>
                                                            <%-- <asp:DropDownList runat="server" class="form-control" Width="100%" ID="ddlcitys">
                                                     <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                </asp:DropDownList>--%>
                                                        </div>

                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Pincode</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ReadOnly="true" Width="100%" ID="txtpin" MaxLength="6"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Personal Email Id</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtemail" PlaceHolder="Email ID" ReadOnly="true" Width="100%" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Contact No. </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtcontact" PlaceHolder="Enter contact No." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Contact</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="txtshopcontact" PlaceHolder="Enter landline no." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Address </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox1" PlaceHolder="Enter landline no." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                                        </div>


                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop State </label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox2" ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>

                                                        </div>
                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Pincode</label>
                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="rectextshopoin" ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>
                                                        </div>



                                                        <div class="col">
                                                            <label for="exampleInputEmail1">Shop Country</label>

                                                            <asp:TextBox runat="server" CssClass="input-text form-control" ID="TextBox3" PlaceHolder="Enter landline no." ReadOnly="true" Width="100%" MaxLength="10"></asp:TextBox>

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
                                                                    <img class="img-fluid" id="img1" src="<%=pathId%>" alt="trainer-img" style="height: 86px;width: 128px;" /></center>
                                                                <ul>
                                                                    <li>
                                                                        <asp:ImageButton ID="btnViewDownloadID" ImageUrl="../../images/download.png"  runat="server" Width="36px" Height="36px" Style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="btnViewDownloadDoc_Click" />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>

                                                                        <%--<a href="#"><i class="fa fa-download"></i></a>--%>
                                                                    </li>
                                                                    <li>
                                                                         <%--<i class="fa fa-eye" id="IDen"  runat="server" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;" onclick="SetValue('IdProof');"></i>--%>
                                                                       
                                                                        <asp:ImageButton ID="btnViewID" ImageUrl="../../images/eyeview.png" runat="server"  Width="36px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClientClick="SetValue('IdProof');" />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-eye"></i></button>--%>
                                                                        <%--<a href="#"><i class="fa fa-eye"></i></a>--%>
                        
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
                                                                         <img class="img-fluid" src="<%=PathAdd%>" alt="trainer-img"  style="height: 86px;width: 128px;" /></center>
                                                                <ul>
                                                                    <li>
                                                                         <asp:ImageButton ID="btnViewDownloadAdd"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="btnViewDownloadDoc_Click1"  />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                    </li>
                                                                    <li>
                                                                        <%-- <i class="fa fa-eye" style="width:49px; height:36px; margin-left: -43px; margin-top: 9px; font-size:35px;"  onclick="SetValue('AddProof');"></i>--%>
                                                                         
                                                                          <asp:ImageButton ID="btnViewAdd" ImageUrl="../../images/eyeview.png" runat="server" Width="36px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClientClick="SetValue('AddProof');" />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-eye"></i></button>--%>
                                                                        <%--<a href="#"><i class="fa fa-eye"></i></a>--%>
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
                                                                          <img class="img-fluid" src="<%=PathSig%>" alt="trainer-img" style="height: 86px;width: 128px;"  /></center>
                                                                <ul>
                                                                    <li>
                                                                         <asp:ImageButton ID="btnViewDownloadSig"  ImageUrl="../../images/download.png" runat="server" Width="36px" Height="36px" style="margin-left: -41px; margin-top: 9px;" ToolTip="View Doc" OnClick="btnViewDownloadDoc_Click2"/>
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px; padding-bottom: 10px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-download"></i></button>--%>
                                                                        <%--  <a href="#"><i class="fa fa-download"></i></a>--%>
                                                                    </li>
                                                                    <li>
                                                                          
                                                                        <asp:ImageButton ID="btnViewSig" ImageUrl="../../images/eyeview.png" runat="server" Width="36px" Height="36px" ToolTip="View Doc" Style="margin-left: -43px; margin-top: 9px;" OnClientClick="SetValue('SigProof');" />
                                                                        <%--<button type="button" style="font-size: 35px; background-color: transparent; border: none; width: 5px; height: 5px;" runat="server" onserverclick="Unnamed_ServerClick"><i style="color: white; width: 5px; height: 5px;" class="fa fa-eye"></i></button>--%>
                                                                        <%--   <a href="#"><i class="fa fa-eye"></i></a>--%>
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
                                                                <label for="exampleInputEmail1">Confirmation on all above Agent Details are properly filled</label>
                                                            </div>
                                                            </b>
                                                        </div>
                                                    </div>

                                                    <hr class="hr-line">

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                        <%--<button type="button" id="downloadPass" runat="server" visible="true" OnClick="" class="btn btn-success">Submit</button>--%>
                                                        <asp:Button runat="server" ID="downloadPass" CssClass="themeBtn themeApplyBtn" OnClick="downloadPass_Click" ToolTip="Submit" Text="Submit" OnClientClick="Confirm3();"></asp:Button>
                                                        <asp:Button runat="server" ID="btnCloseReceipt" CssClass="themeBtn resetBtn themeCancelBtn me-0r" OnClick="btnCloseReceipt_Click" OnClientClick="Confirm();" ToolTip="Close" Text="Back"></asp:Button>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-99" id="divPaymentReceipt" runat="server" visible="false" style="margin-left: 17%; margin-right: 189px;">
                                    <asp:Panel ID="Panel2" runat="server" Width="100%" Style="margin: 5px 0px 0px 0px; padding: 10px; border: 1px solid gray;">
                                        <div class="panel-body Receipt project-name" id="Recipt">

                                            <div class="form-group row">
                                                <div class="col-sm-12">
                                                    <%--<div class="col-sm-4">
                                <div class="form-group row" runat="server">
                                    <div class="col-sm-12 col-xm-12">
                                        <img src="../images/PayRakam.png" height="54" />
                                    </div>
                                </div>
                            </div>--%>

                                                    <div style="margin-left: 84%;">
                                                        <img src="../images/PayRakam.png" height="24" />

                                                    </div>
                                                </div>

                                                <div class="col-sm-12">

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">

                                                        <div class="col2">
                                                            <h5 class="text-Head"><u><strong>General Information</strong></u></h5>
                                                        </div>
                                                        <div class="col3"></div>

                                                        <div class="col4">
                                                            <h5 class="text-Head"><u><strong>Communication Details</strong></u></h5>
                                                        </div>

                                                    </div>

                                                </div>
                                                <div style="height: 10px"></div>

                                                <div class="col-sm-12">

                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap" runat="server" id="div1">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Agent Name:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblbcname"></asp:Label>
                                                            <%--Text="omkar manohar jadhav"--%>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Address:</label>
                                                        </div>
                                                        <div class="col4">
                                                            <asp:Label runat="server" ID="lbladdress"></asp:Label><%-- Text="omkar manohar jadhav xcgdfbvgfdgsfdgfdgfdgfdgfdgfgfgfdgfdgfd retrgrd tgret"--%>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px" id="div6">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Gender:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblgender" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Country:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblcountry" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px" id="div2">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Pan No.:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblpan" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">State:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblstate" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px" id="div3">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">GST No:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblgst" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">District:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lbldist" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Aadhar No:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblaadh" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">City:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblcity" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px" id="div5">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Agent Category:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblcategory" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">PinCode:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblpincode" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px">
                                                        <%--   <div class="col2">
                                            <label for="exampleInputEmail1">Org. Type:</label>
                                        </div>--%>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblorgtype" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Email-Id:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblemail" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Account No:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblaccountno" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">Contact No.:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblcontactno" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row" runat="server" style="margin-bottom: 10px" id="div4">
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">IFSC:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lblifsc" ReadOnly="true"></asp:Label>
                                                        </div>
                                                        <div class="col2">
                                                            <label for="exampleInputEmail1">LandLine No.:</label>
                                                        </div>
                                                        <div class="col3">
                                                            <asp:Label runat="server" ID="lbllandline" ReadOnly="true"></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div style="height: 20px"></div>
                                                <div class="col-sm-77">
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y">
                                                        <b>
                                                            <label for="exampleInputEmail1">Note: </label>
                                                        </b>

                                                        <b>
                                                            <label for="exampleInputEmail1">Confirmation on all above Agent Deatils are properly filled</label>
                                                        </b>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="panel-footer">
                                            <div>
                                                <%--<button type="button" id="downloadPass" runat="server" visible="true" onclick="printdiv('Recipt');" class="btn btn-success">Submit</button>--%>
                                                <%--<asp:Button runat="server" ID="btnCloseReceipt" CssClass="btn btn-danger" OnClick="btnCloseReceipt_Click" ToolTip="Close" Text="Cancel"></asp:Button>--%>
                                                <%--OnClick="btnCloseReceipt_Click"--%>
                                                <asp:Button runat="server" ID="btnConfirm" Visible="false" CssClass="btn btn-success" ToolTip="Close" Text="Cancel"></asp:Button>
                                                <%--OnClick="btnConfirm_Click"--%>
                                            </div>
                                        </div>
                                        <asp:Button ID="btnChange" runat="server" UseSubmitBehavior="false" Style="display: none" /><br />
                                        <br />
                                    </asp:Panel>
                                </div>

                                <%--<asp:HiddenField ID="HidBCID" runat="server" />--%>
                                <asp:HiddenField ID="HidAGID" runat="server" />
                            </div>

                        </div>
                        <div id="BulkAgentReg" class="tab-pane fade">
                            <asp:Panel ID="pnlAddPages" runat="server" Width="100%" Style="overflow: hidden;">
                                <div class="accordion summary-accordion" id="history-accordion">
                                    <div class="accordion-item">
                                        <div class="accordion-header d-flex align-items-center justify-content-between" id="headingSummaryOne">
                                            <h6 class="searchHeader-heading">Filter</h6>
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
                                                    <div class="col col-all-filter">
                                                        <label class="selectInputLabel" for="selectInputLabel">From Date</label>
                                                        <div class="selectInputDateBox w-100">
                                                            <input type="date" runat="server" id="txtFromDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkDate()" />
                                                        </div>
                                                    </div>
                                                    <div class="col col-all-filter">
                                                        <label class="selectInputLabel" for="selectInputLabel">To Date</label>
                                                        <div class="selectInputDateBox w-100">
                                                            <input type="date" runat="server" id="txtToDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" onchange="checkToDate()" />
                                                        </div>
                                                    </div>
                                                    <!-- input -->
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">File Status</label>
                                                        <div class="selectInputBox">
                                                            <asp:DropDownList runat="server" ID="ddlFileTypeStatus" CssClass="maximus-select w-100" TabIndex="1" AppendDataBoundItems="true" AutoPostBack="False">
                                                                <asp:ListItem Value="-1" Text="-- File Status --"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Success"></asp:ListItem>
                                                                <asp:ListItem Value="0" Text="Failed"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <!-- input -->
                                                    <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                        <button type="button" id="btnUpSearch" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnUpSearch_ServerClick">Search</button>
                                                        <button type="button" id="btnUpClear" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" onserverclick="btnUpClear_ServerClick">Clear</button>
                                                    </div>
                                                </div>

                                                <br />
                                                <h6 class="searchHeader-heading">File Upload</h6>
                                                <br />
                                                <hr class="hr-line">
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap">
                                                    <div class="col col-all-filter" style="display: none">
                                                        <label class="selectInputLabel" for="selectInputLabel">Date</label>
                                                        <div class="selectInputDateBox w-100">
                                                            <input type="date" runat="server" id="txtDate" class="multiple-dates select-date form-control" style="width: 100%" placeholder="Select Date" />
                                                        </div>
                                                    </div>

                                                    <%--<div class="col">
                                                    <label class="selectInputLabel" for="selectInputLabel">BC</label>
                                                    <div class="selectInputBox">
                                                        <asp:DropDownList runat="server" ID="ddlBCCode" CssClass="maximus-select w-100" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>--%>
                                                    <div class="col">
                                                        <label class="selectInputLabel" for="selectInputLabel">File Upload</label>
                                                        <div class="selectInputBox">
                                                            <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" Height="33px" Width="270px" Font-Size="Small" />
                                                        </div>
                                                        <asp:Label ID="Label5" runat="server" Font-Size="smaller" Font-Bold="true" Text="All the columns should be in text format only." CssClass="err"></asp:Label>
                                                    </div>

                                                    <div class="col" style="margin-left: 90px">
                                                        <label class="selectInputLabel" for="selectInputLabel">Document Upload</label>
                                                        <div class="selectInputBox">
                                                            <asp:FileUpload ID="DocUpload" runat="server" CssClass="form-control" Height="33px" Width="270px" Font-Size="Small" />
                                                            <asp:Label ID="Label6" runat="server" Font-Size="smaller" Font-Bold="true" Text="All documents should be formatted as - TerminalID_DocID" CssClass="err"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <%--<div class="col-sm-6 col-xs-4 cssRecordsTotal" style="margin-left: 234px">
                                                    <center><strong><asp:Label ID="lblRecordCount" CssClass="form-contol" runat="server" Text=""></asp:Label></strong></center>
                                                </div>--%>
                                                </div>
                                                <div class="row row-cols-auto selectInput-grid20 selectGrid-m-y select-grid-gap searchbox-btns">
                                                    <%--<button type="button" id="btnBulkUpload" runat="server" class="themeBtn themeApplyBtn" data-bs-toggle="modal" onserverclick="btnBulkUpload_ServerClick" onclick="return postbackButtonClick();">Upload</button>--%>
                                                    <asp:Button runat="server" ID="btnBulkUpload" Text="Upload" CssClass="themeBtn themeApplyBtn" OnClick="btnBulkUpload_ServerClick" OnClientClick="return postbackButtonClick();" />
                                                    <button type="button" id="btnDocUpload" runat="server" visible="false" class="themeBtn themeApplyBtn" data-bs-toggle="modal" onserverclick="btnDocUpload_ServerClick">Document Upload</button>
                                                    <button type="button" id="btnUploadCancel" runat="server" class="themeBtn resetBtn themeCancelBtn me-0" data-bs-toggle="modal" onserverclick="btnUploadCancel_ServerClick">Cancel</button>
                                                    <button type="button" id="btnsample" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnsample_ServerClick">Reg. Sample</button>
                                                    <button type="button" id="btnZipsample" runat="server" class="themeBtn themeApplyBtn" onserverclick="btnZipsample_ServerClick">Document Sample</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="table-box">
                                        <div class="HeaderStyle gv-responsive">
                                            <asp:GridView ID="gvBulkManualKyc" runat="server"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                AllowPaging="true"
                                                CssClass="GridView"
                                                PageSize="10"
                                                Visible="true"
                                                DataKeyNames="FileID,FileStatus"
                                                OnRowCommand="gvBulkManualKyc_RowCommand"
                                                AllowSorting="True"
                                                OnPageIndexChanging="gvBulkManualKyc_PageIndexChanging">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                <RowStyle ForeColor="Black" Height="22px" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" Wrap="false"></RowStyle>
                                                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="black" Height="25px" Wrap="false"></HeaderStyle>
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
                                                <PagerStyle CssClass="pagination-ys" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Action" HeaderStyle-CssClass="text-center">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpButtons" runat="server">
                                                                <ContentTemplate>

                                                                    <asp:ImageButton ID="btnViewDownload" runat="server" ImageUrl="../../images/download.png" Width="16" Height="16"
                                                                        CommandName="DownloadDoc" ToolTip="Download Document" CommandArgument='<%# Eval("FileID") %>' />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btnViewDownload" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="FileID" HeaderText="File ID" ItemStyle-HorizontalAlign="Center" SortExpression="FileID" HeaderStyle-BackColor="Black" />
                                                    <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" HeaderStyle-BackColor="Black" />
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="Imported By" SortExpression="CreatedBy" HeaderStyle-BackColor="Black" />
                                                    <asp:BoundField DataField="CreatedOn" HeaderText="Imported On" SortExpression="CreatedOn" HeaderStyle-BackColor="Black" />
                                                    <%--<asp:BoundField DataField="FileStatus" HeaderText="File Status" SortExpression="FileStatus" HeaderStyle-BackColor="Black" />--%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <asp:HiddenField ID="hidimgId" runat="server" />
                    <asp:HiddenField ID="hidimgAdd" runat="server" />
                    <asp:HiddenField ID="hidimgSig" runat="server" />
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnSubmitDetails" EventName="click" />
                <asp:PostBackTrigger ControlID="BtnSubmit" />
                <asp:PostBackTrigger ControlID="btnViewDownloadId" />
                <%--<asp:AsyncPostBackTrigger ControlID="btnViewId" EventName="click" />--%>
                <%-- <asp:PostBackTrigger ControlID="btnViewId" />--%>
                <asp:PostBackTrigger ControlID="btnViewDownloadAdd" />
                <%-- <asp:PostBackTrigger ControlID="btnViewAdd" />--%>
                <asp:PostBackTrigger ControlID="btnViewDownloadSig" />
                <%--  <asp:PostBackTrigger ControlID="btnViewSig" />--%>
                <%--    <asp:AsyncPostBackTrigger ControlID="btnAddnew" EventName="click" />--%>
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                <%--     <asp:PostBackTrigger ControlID="btnAddnew" />--%>
                <%--<asp:PostBackTrigger ControlID="downloadPass" />--%>
                <%--           <asp:PostBackTrigger ControlID="BtnBack" />--%>

                <asp:PostBackTrigger ControlID="btnExportCSV" />
                <asp:PostBackTrigger ControlID="btndownload" />

                <%--<asp:AsyncPostBackTrigger ControlID="btnBulkUpload" />--%>
                <asp:PostBackTrigger ControlID="btnBulkUpload" />
                <asp:PostBackTrigger ControlID="btnsample" />
                <asp:PostBackTrigger ControlID="btnZipsample" />
                <asp:PostBackTrigger ControlID="btnDocUpload" />
                <%--<asp:PostBackTrigger ControlID="BtnCsv" />
                <asp:PostBackTrigger ControlID="BtnXls" />--%>
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
                <asp:Button ID="Button2" runat="server" CausesValidation="false" class="btn btn-danger" Text="Cancel" OnClick="Button2_Click"></asp:Button>
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

    <cc1:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="upPanel" PopupControlID="upContentBodyUpdateProgress" DropShadow="false" />
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
                    $("#<%=ddlclient.ClientID%>").select2();
                    $("#<%=ddlGender.ClientID%>").select2();
                    $("#<%=ddlbcCode.ClientID%>").select2();
                    $("#<%=ddlbcCode.ClientID%>").select2();
                    $("#<%=ddlState.ClientID%>").select2();
                    $("#<%=ddlCity.ClientID%>").select2();
                    $("#<%=ddlCategory.ClientID%>").select2();

                    $("#<%=ddlCountry.ClientID%>").select2();
                    $("#<%=ddlAddressProof.ClientID%>").select2();
                    $("#<%=ddlIdentityProof.ClientID%>").select2();
                    $("#<%=ddlSignature.ClientID%>").select2();
                    $("#<%=ddlshopCountry.ClientID%>").select2();
                    $("#<%=ddlShopCity.ClientID%>").select2();
                    $("#<%=ddlShopState.ClientID%>").select2();
                    $("#<%=ddlarea.ClientID%>").select2();

                    $("#<%=ddlRequestType.ClientID%>").select2();
                    $("#<%=ddlBucketId.ClientID%>").select2();
                    $("#<%=ddlRequestStatus.ClientID%>").select2();
                    $("#<%=ddlFileTypeStatus.ClientID%>").select2();


                    $("#<%=ddlDistrict.ClientID%>").select2();
                    $("#<%=ddlShopDistrict.ClientID%>").select2();
                    $("#<%=ddlaggregatorCode.ClientID%>").select2();



                }
            });
        };
    </script>
    <script type="text/javascript">  
        function checkDate() {
            var date = document.getElementById("<%= txtFromDate.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtFromDate.ClientID %>").value = todayDate;
            }
            else
                document.getElementById("datePicker").value = todayDate;
        }
    </script>
    <script type="text/javascript">  
        function checkToDate() {
            var date = document.getElementById("<%= txtToDate.ClientID %>").value;
            var todayDate = new Date().toISOString().slice(0, 10);
            if (date > todayDate) {
                alert("Selected date can not be greater than Current date !!");
                document.getElementById("<%= txtToDate.ClientID %>").value = todayDate;
            }
            else
                document.getElementById("datePicker").value = todayDate;
        }
    </script>
    <%--<script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= upContentBodyUpdateProgress.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
</script>--%>
</asp:Content>

