$(document).ready(function () {
    var baseUrl = "http://localhost:52315/";

    var eventid;
    if ($('.linkedinlogin').length > 0)
        eventid = $('.linkedinlogin').attr("id").split('_')[1];
    else
        eventid = 0;

    var userid = getUrlVars()["userid"];
    var accesstoken = getUrlVars()["accesstoken"];
    var profileid = getUrlVars()["profileid"];
    var eid = getUrlVars()["eid"];
    var referralid = getUrlVars()["r"];
    var exists = getUrlVars()["exists"];
    if (referralid === "" || referralid === null || referralid === undefined) {
        referralid = 0;
    }
    
    //if user has successfully authenticated with linkedIn, Its time to render form
    if ($('.eventregistration').length > 0 && userid !== undefined)
    {
        $.ajax({
            method: 'POST',
            url: baseUrl + '/Home/GetEventDetails',
            data: {
                EventKey: eid
            },
            beforeSend: function () { },
            success: function (response) {
                if (response.success) {
                    Formio.createForm(document.getElementById('eventregistration'), JSON.parse(response.form))
                        .then(function (form) {
                            form.on('submit', function (submission) {
                                submission.data.EventKey = eid;
                                submission.data.ReferralId = referralid;
                                console.log(submission);
                                SaveUser(submission);

                                $.ajax({
                                    method: 'POST',
                                    url: baseUrl + '/Home/SubmitData',
                                    data: {
                                        collection: "EventRegistration",
                                        data: JSON.stringify(submission)
                                    },
                                    success: function (response) {
                                        form.emit('submitDone', submission);
                                        response.json();
                                    }
                                });
                            });
                        });

                    $('#eventregistration').append("<input type='hidden' id='EventKey' value='" + eid + "' />"
                        + "<input type='hidden' id='accesstoken' value='" + accesstoken + "' />"
                        + "<input type='hidden' id='profileid' value='" + profileid + "' />"
                        + "<table id='tblData' style='display:none;max-width: 350px; background: #fff;padding: 40px; font-family:arial;'><tbody>"
                        + "<tr><td>Profile Image</td><td><img id='profileImage' /></td></tr>"
                        + "<tr><td>First Name</td><td><input type='text' id='lblFirstName' name='lblFirstName' style='height: 30px;border: 1px solid #ccc;margin-bottom: 5px;'></td></tr>"
                        + "<tr><td>Last Name</td> <td><input type='text' id='lblLastName' name='lblLastName' style='height: 30px;border: 1px solid #ccc;margin-bottom: 5px;'></td></tr>"
                        + "<tr><td>Email</td><td><input type='text' id='lblEmail' name='lblEmail' style='height: 30px;border: 1px solid #ccc;margin-bottom: 5px;;'></td></tr>"
                        + "</table>"
                    );

                    GetRegisteredUser();
                }
            }
        });
    }
    else if ($('.linkedinlogin').length > 0)
    {
        $('.linkedinlogin').html(
            "<a id='btnLinkedIn' target='blank' href='https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=81wyrvvax51otg&redirect_uri=http%3A%2F%2Fwww.geteventpro.com%2F%3Feid%3D" + eventid + "%26r%3D" + referralid + "&state=987654321&scope=r_emailaddress%20w_share%20r_basicprofile%20r_liteprofile%20rw_company_admin%20w_member_social'>"
            + "<img src='" + baseUrl + "/Linkedin-customized-button.png'></a>"
        );
    }

    function GetRegisteredUser() {
    	$.ajax({
    		method: 'POST',
    		url: baseUrl + '/Home/GetRegisteredUser',
    		data: {
    			UserId: userid
    		},
    		beforeSend: function () { },
            success: function (response) {
    			if (response.success) {
                    $('.linkedinlogin').hide();
                    $('.eventregistration').show();
    				$('#lblFirstName').val(response.user.FirstName);
    				$('#lblLastName').val(response.user.LastName);
    				$('#lblEmail').val(response.user.Email);
                    $('#profileImage').attr("src", response.user.ProfileImage);
    			}
    		}
    	});
    }

    function SaveUser(submission) {
        $.ajax({
            method: 'POST',
            url: baseUrl + '/Home/RegisterUser',
            data: {
                accesstoken: $('#accesstoken').val(),
                profileid: $('#profileid').val(),
                EventKey: $('#EventKey').val(),
                UserId: userid,
                FirstName: $('#lblFirstName').val(),
                LastName: $('#lblLastName').val(),
                Email: $('#lblEmail').val()
            },
            success: function (response) {
                console.log(response);
            }
        });
    }

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
});