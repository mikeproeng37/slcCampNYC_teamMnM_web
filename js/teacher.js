function displaySections(data) {
    var numSections = data.length;
    for (var i = 0; i < numSections; i++) {
        var section = data[i];
        var sectionOp = $("<option>");
        $(sectionOp).val(section.id);
        $(sectionOp).text(section.uniqueSectionCode);

        $("#sectionList").append(sectionOp);
    }
    $("#sectionList").chosen();
};

$(document).ready(function () {
    GetData("Teacher.aspx/GetSections", {}, displaySections);
    getAssignments();
    $('#eventDate').datepicker();
    $('#eventDate').datepicker("option", "dateFormat", 'yy-mm-dd');
});

function showSectionInfo() {
    var id = $("#sectionList").val();
    window.location.href = "/section.aspx?id=" + id;
}

function showStudents() {
    var id = $("#sectionList").val();
    getStudentsBySectionId(id);
}

function displayStudents(data) {
    var length = data.length;
    var studentList = $("#students");
    $('#studentData').empty();
    $('#students').empty();
    for (var i = 0; i < length ; i++) {
        studentList.append('<li class="student"><a onclick="getStudentAssessmentsByStudentId(\'' + data[i].id + '\');">' + data[i].name.firstName + ' ' + data[i].name.lastSurname + '</a></li>');
    }
}

function displayStudentAssessments(data) {
    var length = data.length;
    $('#studentData').empty();
    var noteMarkup = '<div id="createNewNote"><h2>Record a note</h2></br>*Note: <input style="margin-left: 42px" type="text" id="noteTitle"/> <br />';
    noteMarkup += 'more detail: <input type="text" id="noteDescription"/> <br />';
    noteMarkup += '<button onclick="createNoteNews(\'' + data[0].studentAssessmentAssociation[0].studentId + '\'); return false;" class="btn">Create</button></div>';
    $('#studentData').append(noteMarkup);
    $('#studentData').append("<h2>Grade student assessments</h2></br>");
    $('#studentData').append('<ul id="assessments"></ul>');
    for (var i = 0; i < length ; i++) {
        var markup = '<li>' + data[i].assessmentTitle + ': ';
        if (data[i].studentAssessmentAssociation[0].scoreResults !== undefined && data[i].studentAssessmentAssociation[0].scoreResults.length > 0) {
            markup += '<input style="margin-right: 10px" type="text" id="' + data[i].studentAssessmentAssociation[0].id + '" value="' + data[i].studentAssessmentAssociation[0].scoreResults[0].result + '"></input>' + '/' + data[i].maxRawScore; //add text box to update score
        } else {
            markup += getTextBox(data[i].id) + '/' + data[i].maxRawScore;       
        }

        markup += '<button style="margin-left: 10px" class="btn btn-success" onclick="saveStudentScore(\'' + data[i].studentAssessmentAssociation[0].id + '\', \'' + data[i].studentAssessmentAssociation[0].studentId + '\', ';
        markup += '\'' + data[i].id + '\', \'' + data[i].studentAssessmentAssociation[0].administrationDate + '\', ';
        markup += '\'' + data[i].studentAssessmentAssociation[0].id + '\', ';
        markup += '\'' + data[i].assessmentTitle + '\', ';
        markup += '\'' + data[i].maxRawScore + '\' ';
        markup += ')">Save</button></li>';

        $('#assessments').append(markup);
    }
}

function getStudentsBySectionId(id) {
    GetData("Teacher.aspx/GetStudentsBySectionId", { sectionId: id }, displayStudents);
}

function getStudentAssessmentsByStudentId(id) {
    GetData("Teacher.aspx/GetStudentAssessmentByStudentId", { studentId: id }, displayStudentAssessments);
}

function getTextBox(id) {
    return '<input style="margin-right: 10px" type="text", id=\'' + id + '\'></input>'
}

function saveStudentScore(said, sid, aid, adminDate, inputId, aName, max) {
    var scr = $('#' + inputId).val();
    if (scr === undefined || scr === null || scr === '') {
        alert("Please enter a score result first");
    } else {
        //string studentAssessmentId, string studentId, string assessmentId, string administrationDate, string score
        GetData("Teacher.aspx/UpdateStudentAssessmentAssociation", { studentAssessmentId: said, studentId: sid, assessmentId: aid, administrationDate: adminDate, score: scr, assessmentName: aName, maxScore: max },
            successfullyUpdatedStudentAssessmentAssociation);
    } 
}

function successfullyUpdatedStudentAssessmentAssociation(isSuccessful) {
    if (isSuccessful)
        alert('Success!');
    else
        alert('Failure');
}

function getAssignments() {
    GetData("Teacher.aspx/GetAssignments", {}, displayAssignments);
}

function displayAssignments(data) {
    var length = data.length;
    //var studentList = $("#assignments");
    $('#assignments li').remove();
    for (var i = 0; i < length ; i++) {
        //var newSections = $("#sectionList select");
        var markup = '<li>' + data[i].assessmentTitle;
        markup += '<input type="text" id="' + data[i].id + '" style="margin:4px 7px 0px 7px;">';
        markup += '<button onclick="assignAssignmentToStudents(\'' + data[i].id + '\', ';
        markup += '\'' + data[i].assessmentTitle + '\', ';
        markup += '\'' + data[i].id + '\' ';
        markup += ')">Assign</button></li>';

        $('#assignments').append(markup);

        $('#' + data[i].id).datepicker();
        $("#" + data[i].id).datepicker("option", "dateFormat", 'yy-mm-dd');
    }
}

function assignAssignmentToStudents(id, title, inputId) {
    var val = $('#' + inputId).val();
    if (val === undefined || val === null || val === '') {
        alert("Please enter a due date first");
    } else {
        GetData("Teacher.aspx/AssignAssignmentToStudents", {assignmentId: id, assignmentTitle: title, dueDate: val}, succesfullyAssignedAssignments);
    }
}

function succesfullyAssignedAssignments(isSuccessful) {
    if (isSuccessful)
        alert('Success!');
    else
        alert('Failure');
}

function createEventNews() {
    var title = $('#title').val();
    var description = $('#description').val();
    var eventDate = $('#eventDate').val();

    if (title === undefined || title === null || title === '' || description === undefined || description === null || description === '' || eventDate === undefined || eventDate === null || eventDate === '') {
        alert("Please all the required info first");
    } else {
        GetData("Teacher.aspx/createEventNews", { eventTitle: title, eventDescription: description, eventDate: eventDate }, succesfullyCreatedEvent);
    }
}

function succesfullyCreatedEvent(isSuccessful) {
    if (isSuccessful)
        alert('Success!');
    else
        alert('Failure');
}

function createNoteNews(sid) {
    var title = $('#noteTitle').val();
    var description = $('#noteDescription').val();

    if (title === undefined || title === null || title === '') {
        alert("Please all the required info first");
    } else {
        GetData("Teacher.aspx/createNoteNews", {studentId: sid, noteTitle: title, noteDescription: description}, succesfullyCreatedNote);
    }
}

function succesfullyCreatedNote(isSuccessful) {
    if (isSuccessful)
        alert('Success!');
    else
        alert('Failure');
}

function changeTab(tab, selectedTab) {
    $(".content").hide();
    $(selectedTab).show();
    if (selectedTab !== '#studentData')
        $('#students').hide();
    else
        $('#students').show();
}