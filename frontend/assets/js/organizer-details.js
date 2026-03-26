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
    const organizerId = urlParams.get('id');

    fetch(`http://localhost:8000/admin/api/organizer/${organizerId}/details`, {
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
            console.log(data);
            updateOrganizerDetails(data)

        }

        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });

        function updateOrganizerDetails(data){
        
            document.getElementById('organizerName').innerText = data.UserName || "N/A";
            document.getElementById('organizerUserName').innerText = data.Email || "N/A";
            document.getElementById('description').innerText = data.Phone || "N/A";
            document.getElementById('name').innerText = data.FirstName || "N/A";
            document.getElementById('eventType').innerText = data.LastName || "N/A";
            document.getElementById('state').innerText = data.OtherName || "N/A";
            document.getElementById('location').innerText = data.State || "N/A";
            document.getElementById('startDate').innerText = data.LGA || "N/A";
            document.getElementById('endDate').innerText = data.Address || "N/A";
            document.getElementById('isInvitationOnly').innerText = data.Gender || "N/A" ;
            document.getElementById('hasPayment').innerText = data.UserType || "N/A";
            document.getElementById('isBlock').innerText = data.isBlock ? "Yes" : "No";
            document.getElementById('createdAt').innerText = new Date(data.createdAt).toLocaleString() || "N/A";

        }
    







})