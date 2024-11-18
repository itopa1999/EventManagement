document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('event_token');
    if (!token) {
        window.location.href = 'login.html';
    }

    function formatCurrency(value) {
        if (value === undefined || value === null) {
            return '₦0.00';
        }
        return '₦' + value.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }

    const errorAlert = document.getElementById('error-alert');
    const errorMessage = document.getElementById('error-message');
    const successAlert = document.getElementById('success-alert');
    const successMessage = document.getElementById('success-message');

    // Reset previous messages
    errorAlert.classList.add('d-none');
    errorMessage.innerHTML = '';
    successAlert.classList.add('d-none');
    successMessage.innerHTML = '';

    const urlParams = new URLSearchParams(window.location.search);
    const eventId = urlParams.get('id');

    function fetchEventDetails(){
        fetch(`http://localhost:8000/admin/api/event/${eventId}/details`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then(async response => {
            if (response.status === 401 || response.status === 403) {
                errorMessage.innerText = 'Unauthorized access. Redirecting to login...';
                errorAlert.classList.remove('d-none');
                setTimeout(function() {
                    window.location.href = 'login.html'; // Redirect after 4 seconds
                }, 4000);
            } else if (response.status === 400) {
                const data = await response.json();
                errorMessage.innerText = data.error_description || 'There was a bad request';
                errorAlert.classList.remove('d-none');
            } else if (!response.ok) {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
                
            } else {
                // spinner.style.display = 'none';
                const data = await response.json();
                updateEventDetails(data)
                updateEventSessions(data.sessions)

            }

        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });
    }



    function updateEventDetails(data){
        console.log(data);
        
        document.getElementById('eventImage').src = data.imagePath;
        document.getElementById('organizerName').innerText = data.organizerFName + " " + data.organizerLName || "N/A";
        document.getElementById('organizerUserName').innerText = data.organizerUName || "N/A";
        document.getElementById('description').innerText = data.description || "N/A";
        document.getElementById('name').innerText = data.name || "N/A";
        document.getElementById('eventType').innerText = data.eventType || "N/A";
        document.getElementById('state').innerText = data.state || "N/A";
        document.getElementById('location').innerText = data.location || "N/A";
        document.getElementById('startDate').innerText = new Date(data.startDate).toLocaleString() || "N/A";
        document.getElementById('endDate').innerText = new Date(data.endDate).toLocaleString() || "N/A";
        document.getElementById('isInvitationOnly').innerText = data.isInvitationOnly ? "Yes" : "No";
        document.getElementById('hasPayment').innerText = data.hasPayment ? "Yes" : "No";
        document.getElementById('price').innerText = formatCurrency(data.price) || "N/A";
        document.getElementById('isBlock').innerText = data.isBlock ? "Yes" : "No";
        document.getElementById('createdAt').innerText = new Date(data.createdAt).toLocaleString() || "N/A";

        document.getElementById('organizerDetails').addEventListener('click', function() {
            window.location.href = `organizer-details.html?id=${data.organizerId}`
        })
    }

    function updateEventSessions(data){
        const sessionBody = document.getElementById('session-container');
        sessionBody.innerHTML = '';
        if (data.length === 0) {
            sessionBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center">No data found</td>
                </tr>
            `;
        } else {
            data.forEach((session) => {
                const sessionHtml = `
                <div class="col-md-6 mb-4">
                    <div class="card shadow-sm">
                        <div class="card-body">
                        <h5 class="card-title" id="title1">${session.title}</h5>
                        <p class="card-text">
                            <strong>Start Time:</strong> <span "> ${new Date(session.startTime).toLocaleString()}</span><br>
                            <strong>End Time:</strong> <span">${new Date(session.endTime).toLocaleString()}</span><br>
                            <strong>Speaker:</strong> <span id="speaker1">${session.speaker}</span>
                        </p>
                        </div>
                    </div>
                </div>
                    `;
                    sessionBody.innerHTML += sessionHtml;
            })
        }
    }

    fetchEventDetails();







})