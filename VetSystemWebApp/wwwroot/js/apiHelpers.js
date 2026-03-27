window.getAppointments = async function () {
    const token = localStorage.getItem("token");
    if (!token) return [];

    const response = await fetch("https://localhost:7146/api/Client/Appointments", {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (!response.ok) return [];
    return await response.json();
};