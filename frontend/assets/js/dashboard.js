document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('event_token');
    
    if (!token) {
        window.location.href = 'login.html';
    }

    // Format the currency value
    function formatCurrency(value) {
        if (value === undefined || value === null) {
            return '₦0.00';
        }
        if (value >= 1000000) {
            return '₦' + (value / 1000000).toFixed(1).replace(/\.0$/, '') + 'M+';
        } else if (value >= 1000) {
            return '₦' + (value / 1000).toFixed(1).replace(/\.0$/, '') + 'K+';
        } else {
            return '₦' + value.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }
    }

    let userCurrentChart = null;
    let eventCurrentChart = null;
    let sessionCurrentChart = null;

    // Handle dropdown changes and trigger fetchDashboardData with updated queryParams
    document.getElementById('userYearSelect').addEventListener('change', function() {
        const userYear = document.getElementById('userYearSelect').value || '';
        const eventYear = document.getElementById('eventYearSelect').value || '';
        const sessionYear = document.getElementById('sessionYearSelect').value || '';

        // Create updated queryParams
        const queryParams = new URLSearchParams({
            UserYear: userYear,
            EventYear: eventYear,
            SessionYear: sessionYear
        });

        // Destroy the existing chart if it exists
        if (userCurrentChart) {
            userCurrentChart.destroy();
        }
        if (eventCurrentChart) {
            eventCurrentChart.destroy();
        }
        if (sessionCurrentChart) {
            sessionCurrentChart.destroy();
        }

        // Fetch updated dashboard data
        fetchDashboardData(queryParams);
    });

    document.getElementById('eventYearSelect').addEventListener('change', function() {
        const userYear = document.getElementById('userYearSelect').value || '';
        const eventYear = document.getElementById('eventYearSelect').value || '';
        const sessionYear = document.getElementById('sessionYearSelect').value || '';

        // Create updated queryParams
        const queryParams = new URLSearchParams({
            UserYear: userYear,
            EventYear: eventYear,
            SessionYear: sessionYear
        });

        // Destroy the existing chart if it exists
        if (eventCurrentChart) {
            eventCurrentChart.destroy();
        }
        if (userCurrentChart) {
            userCurrentChart.destroy();
        }
        if (sessionCurrentChart) {
            sessionCurrentChart.destroy();
        }

        // Fetch updated dashboard data
        fetchDashboardData(queryParams);
    });

    document.getElementById('sessionYearSelect').addEventListener('change', function() {
        const userYear = document.getElementById('userYearSelect').value || '';
        const eventYear = document.getElementById('eventYearSelect').value || '';
        const sessionYear = document.getElementById('sessionYearSelect').value || '';

        // Create updated queryParams
        const queryParams = new URLSearchParams({
            UserYear: userYear,
            EventYear: eventYear,
            SessionYear: sessionYear
        });

        // Destroy the existing chart if it exists
        if (sessionCurrentChart) {
            sessionCurrentChart.destroy();
        }
        if (eventCurrentChart) {
            eventCurrentChart.destroy();
        }
        if (userCurrentChart) {
            userCurrentChart.destroy();
        }

        // Fetch updated dashboard data
        fetchDashboardData(queryParams);
    });

    const loadingSpinners = {
        totalProduct: document.getElementById('loadingSpinnerTotalEvents'),
        totalProductItem: document.getElementById('loadingSpinnerTotalSessions'),
        totalProductInStock: document.getElementById('loadingSpinnerActiveEvents'),
        totalProductOutOfStock: document.getElementById('loadingSpinnerActiveSessions'),
        totalUser: document.getElementById('loadingSpinnerOrganizers'),
        totalProductPrice: document.getElementById('loadingSpinnerAttendee'),
        totalProductSoldPrice: document.getElementById('loadingSpinnerTicketSold')
    };

    Object.values(loadingSpinners).forEach(spinner => spinner.style.display = 'inline-block');

    // Get elements for displaying alerts and messages
    const errorAlert = document.getElementById('error-alert');
    const errorMessage = document.getElementById('error-message');
    const successAlert = document.getElementById('success-alert');
    const successMessage = document.getElementById('success-message');

    // Reset previous messages
    errorAlert.classList.add('d-none');
    errorMessage.innerHTML = '';
    successAlert.classList.add('d-none');
    successMessage.innerHTML = '';

    function fetchDashboardData(queryParams) {
        // Fetch the dashboard data
        fetch(`http://localhost:8000/admin/api/dashboard?${queryParams.toString()}`, {
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
            } else if (response.ok) {
                Object.values(loadingSpinners).forEach(spinner => spinner.style.display = 'none');
                const data = await response.json();
                document.getElementById('totalEvent').innerHTML = data.totalEvent;
                document.getElementById('totalSession').innerHTML = data.totalSession;
                document.getElementById('totalActiveEvent').innerHTML = data.totalActiveEvent;
                document.getElementById('totalActiveSession').innerHTML = data.totalActiveSession;
                document.getElementById('totalOrganizer').innerHTML = data.totalOrganizer;
                document.getElementById('totalAttendee').innerHTML = data.totalAttendee;
                document.getElementById('totalTicketSold').innerHTML = data.totalTicketSold;

                updateCharts(data);
            } else {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
            }
        })
        .catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });
    }

    function updateCharts(data) {
        // Event chart
        const eventBar = document.querySelector('#eventChart');
        const eventData = {
            label: 'Events',
            counts: data.monthlyEventRegistrations.map(item => item.count),
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)', 'rgba(255, 159, 64, 0.2)', 'rgba(255, 205, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)',
                'rgba(201, 203, 207, 0.2)', 'rgba(255, 159, 64, 0.2)', 'rgba(255, 205, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)'
            ],
            borderColor: [
                'rgb(255, 99, 132)', 'rgb(255, 159, 64)', 'rgb(255, 205, 86)', 'rgb(75, 192, 192)',
                'rgb(54, 162, 235)', 'rgb(153, 102, 255)', 'rgb(201, 203, 207)', 'rgb(255, 159, 64)',
                'rgb(255, 205, 86)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)'
            ]
        };
        const eventLabels = data.monthlyEventRegistrations.map(item => item.month);
        eventCurrentChart = createChart(eventBar, eventData, eventLabels);

        // Session chart
        const sessionBar = document.querySelector('#sessionChart');
        const sessionData = {
            label: 'Sessions',
            counts: data.monthlySessionRegistrations.map(item => item.count),
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)', 'rgba(255, 159, 64, 0.2)', 'rgba(255, 205, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)',
                'rgba(201, 203, 207, 0.2)', 'rgba(255, 159, 64, 0.2)', 'rgba(255, 205, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)'
            ],
            borderColor: [
                'rgb(255, 99, 132)', 'rgb(255, 159, 64)', 'rgb(255, 205, 86)', 'rgb(75, 192, 192)',
                'rgb(54, 162, 235)', 'rgb(153, 102, 255)', 'rgb(201, 203, 207)', 'rgb(255, 159, 64)',
                'rgb(255, 205, 86)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)'
            ]
        };
        const sessionLabels = data.monthlySessionRegistrations.map(item => item.month);
        sessionCurrentChart = createChart(sessionBar, sessionData, sessionLabels);

        // User chart
        const userBar = document.querySelector('#userChart');
        const userData = {
            label: 'Users',
            counts: data.monthlyUserRegistrations.map(item => item.count),
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)', 'rgba(255, 159, 64, 0.2)', 'rgba(255, 205, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)',
                'rgba(201, 203, 207, 0.2)', 'rgba(255, 159, 64, 0.2)', 'rgba(255, 205, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)'
            ],
            borderColor: [
                'rgb(255, 99, 132)', 'rgb(255, 159, 64)', 'rgb(255, 205, 86)', 'rgb(75, 192, 192)',
                'rgb(54, 162, 235)', 'rgb(153, 102, 255)', 'rgb(201, 203, 207)', 'rgb(255, 159, 64)',
                'rgb(255, 205, 86)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)'
            ]
        };
        const userLabels = data.monthlyUserRegistrations.map(item => item.month);
        userCurrentChart = createChart(userBar, userData, userLabels);
    }
    

    function createChart(chartElement, data, labels) {
        return new Chart(chartElement, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: data.label,
                    data: data.counts,
                    backgroundColor: data.backgroundColor,
                    borderColor: data.borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    fetchDashboardData(new URLSearchParams());
});
