<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="teacher.aspx.cs" Inherits="SLC_Classview_CSharp.Teacher" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="vendor/chosen/css/chosen.css" />
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="vendor/chosen/js/chosen.jquery.min.js"></script>
    <script type="text/javascript" src="js/commonUtils.js"></script>
    <script type="text/javascript" src="js/teacher.js"></script>
    <link href="css/ui-lightness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" />
    <script src="vendor/jquery-ui-1.9.0.custom.min.js"></script>
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/teacher.css" rel="stylesheet" />
</head>
<body>
    <style>
        input[type='text'] {
            height: 35px;
            width: 250px
        }
    </style>
    <header>
        <div class="top">
            <select id="sectionList" data-placeholder="Choose a class">
                <option></option>
            </select>
            <button class="btn btn-primary" onclick="showStudents(); return false;" class="btn">Go</button>
            </div>
     </header>
    <ul class="nav">
        <li>
            <img src="img/teacher/home.png" onclick="changeTab(this, '#studentData');" /></li>
        <li>
            <img src="img/teacher/assignment.png" onclick="changeTab(this, '#assignmentsDiv');"/></li>
        <li>
            <img src="img/teacher/22nd.png" onclick="changeTab(this, '#events');"/></li>
    </ul>
    <div>
        <div class="students">
            <ul id="students" style="overflow: scroll; height: 100%;"></ul>
        </div>
    <div id="studentData" class="content">
        <ul id="assessments"></ul>
    </div>
    <div id="assignmentsDiv" class="content" style="display:none">
        <h2>Assign homework to the class</h2>
        <br />
        <ul id="assignments"></ul>
    </div>
    <div id="events" style="display:none" class="content">
        <div class="row">
            <div><h2>Create new school events</h2></div>
        </div>
        <br />
        <div class="row">
            <div class="span2">*Title: </div>
            <div class="span2"><input type="text" id="title"/></div>
        </div>
        <div class="row">
            <div class="span2">*Description: </div>
            <div class="span2"><input type="text" id="description"/></div>
        </div>
        <div class="row">
            <div class="span2">*Event Date: </div>
            <div class="span2"><input type="text" id="eventDate" /></div>
        </div>
        
        <button onclick="createEventNews(); return false;" class="btn">Create</button>
    </div>
        </div>
    <script src="vendor/bootstrap/js/bootstrap.min.js"></script>
</body>
</html>
