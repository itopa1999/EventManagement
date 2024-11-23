document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('event_token');
    if (!token) {
        window.location.href = 'login.html';
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

    let currentPage = 1;
    let eventName = "";
    document.getElementById('filterForm').addEventListener('submit', function(event) {
        event.preventDefault();

        const startDate = document.getElementById('startDate').value;
        const endDate = document.getElementById('endDate').value;
        const eventType = document.getElementById('eventType').value;
        const location = document.getElementById('location').value;
        const hasPayment = document.getElementById('hasPayment').checked;
        const invitationOnly = document.getElementById('invitationOnly').checked;

        function formatDateToISO(dateString) {
            if (!dateString) return null;
            const date = new Date(dateString);

            const year = date.getUTCFullYear();
            const month = String(date.getUTCMonth() + 1).padStart(2, '0');
            const day = String(date.getUTCDate()).padStart(2, '0');

            return `${year}-${month}-${day}T00:00:00Z`;
        }

        const formattedStartDate = formatDateToISO(startDate);
        const formattedEndDate = formatDateToISO(endDate);

        const queryParameters = [
            eventType ? `EventType=${eventType}` : '',
            location ? `Location=${location}` : '',
            formattedStartDate ? `StartDate=${formattedStartDate}` : '',
            formattedEndDate ? `EndDate=${formattedEndDate}` : '',
            hasPayment ? `HasPayment=${hasPayment}` : '',
            invitationOnly ? `IsInvitationOnly=${invitationOnly}` : ''
        ].filter(param => param).join('&');

        fetchEvents(queryParameters);
    });

    document.getElementById('prevPage').addEventListener('click', function(e) {
        e.preventDefault();
        if (currentPage > 1) {
            currentPage--;
            fetchEvents();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function(e) {
        e.preventDefault();
        currentPage++;
        fetchEvents();
    });

    document.getElementById('eventName').addEventListener('input', function() {
        currentPage = 1;
        eventName = document.getElementById('eventName').value;
        fetchEvents();
      });

    

      function fetchEvents(queryParameters = ''){
        fetch(`http://localhost:8000/admin/api/list/events?Name=${eventName}&PageNumber=${currentPage}&${queryParameters}`, {
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
                updateEventList(data);
            
                

            }



        })
        .catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });

      }

      function updateEventList(data){
        const containerBody = document.getElementById('event-container');
        containerBody.innerHTML = '';
        if (data.events.length === 0) {
            containerBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center">No data found</td>
                </tr>
            `;
        } else {
            data.events.forEach((event) => {
                const eventHtml = `
                <div class="col-md-6 col-lg-4">
                <div class="card event-card">
                    <img src="${event.imagePath}" alt="Event Image" class="card-img-top event-image">
                    <div class="card-body">
                        <h5 class="card-title"><a href='event-details.html?id=${event.id}'>${event.name}</a></h5>
                        <p class="card-text">
                            <strong>Type:</strong> ${event.eventType}<br>
                            <strong>State:</strong> ${event.state}<br>
                            <strong>Location:</strong> ${event.location}<br>
                            <strong>Start:</strong> ${new Date(event.startDate).toLocaleString()}<br>
                            <strong>End:</strong> ${new Date(event.endDate).toLocaleString()}<br>
                            <strong>Description:</strong> ${event.description}
                        </p>
                    </div>
                </div>
            </div>
            `;
            containerBody.innerHTML += eventHtml;
            })

        }

        const itemsPerPage = data.pagesize; // Set your items per page here
        const totalPages = Math.ceil(data.events_count / itemsPerPage);
        document.getElementById('remaining').innerText = `${currentPage}/${totalPages}`;

        const nextPageButton = document.getElementById('nextPage');
        const prevPageButton = document.getElementById('prevPage');
        if (currentPage >= totalPages) {
            nextPageButton.style.display = 'none'; // Add class to visually disable
        } else {
            nextPageButton.style.display = 'block'; // Remove class to enable
        }
    
        if (currentPage <= 1) {
            prevPageButton.style.display = 'none'; // Add class to visually disable
        } else {
            prevPageButton.style.display = 'block'; // Remove class to enable
        }
      }


      
      fetchEvents();


})