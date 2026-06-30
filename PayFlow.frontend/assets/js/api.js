// ============================================
//  PayFlow - API Service
//  All backend calls go through here
// ============================================

const API_BASE = "http://localhost:5091";

// ============================================
//  HTTP HELPERS
// ============================================
function getToken() {
    return localStorage.getItem("payflow_token");
}

function getHeaders(isFormData = false) {
    const headers = { "Authorization": `Bearer ${getToken()}` };
    if (!isFormData) headers["Content-Type"] = "application/json";
    return headers;
}

async function handleResponse(response) {
    if (response.status === 401) {
        clearUser();
        window.location.href = getBasePath() + "index.html";
        return;
    }
    const data = await response.json().catch(() => ({}));
    if (!response.ok) {
        throw new Error(data.message || "Something went wrong.");
    }
    return data;
}

// ============================================
//  AUTH
// ============================================
const AuthAPI = {
    async login(username, password) {
        const res = await fetch(`${API_BASE}/api/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });
        return handleResponse(res);
    },

    async changePassword(currentPassword, newPassword) {
        const res = await fetch(`${API_BASE}/api/auth/change-password`, {
            method: "POST",
            headers: getHeaders(),
            body: JSON.stringify({ currentPassword, newPassword })
        });
        return handleResponse(res);
    }
};

// ============================================
//  EMPLOYEES
// ============================================
const EmployeeAPI = {
    async getAll() {
        const res = await fetch(`${API_BASE}/api/employee`, { headers: getHeaders() });
        return handleResponse(res);
    },

    async getById(id) {
        const res = await fetch(`${API_BASE}/api/employee/${id}`, { headers: getHeaders() });
        return handleResponse(res);
    },

    async create(data) {
        const res = await fetch(`${API_BASE}/api/employee`, {
            method: "POST",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return handleResponse(res);
    },

    async update(id, data) {
        const res = await fetch(`${API_BASE}/api/employee/${id}`, {
            method: "PUT",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return handleResponse(res);
    },

    async activate(id) {
        const res = await fetch(`${API_BASE}/api/employee/${id}/activate`, {
            method: "PATCH",
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async deactivate(id) {
        const res = await fetch(`${API_BASE}/api/employee/${id}/deactivate`, {
            method: "PATCH",
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async createAccount(id, username) {
        const res = await fetch(`${API_BASE}/api/employee/${id}/create-account`, {
            method: "POST",
            headers: getHeaders(),
            body: JSON.stringify({ username })
        });
        return handleResponse(res);
    },

    async uploadPhoto(id, file) {
        const formData = new FormData();
        formData.append("file", file);
        const res = await fetch(`${API_BASE}/api/employee/${id}/upload-photo`, {
            method: "POST",
            headers: { "Authorization": `Bearer ${getToken()}` },
            body: formData
        });
        return handleResponse(res);
    }
};

// ============================================
//  ATTENDANCE
// ============================================
const AttendanceAPI = {
    async createOrUpdate(data) {
        const res = await fetch(`${API_BASE}/api/attendance`, {
            method: "POST",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return handleResponse(res);
    },

    async getByMonthYear(month, year) {
        const res = await fetch(`${API_BASE}/api/attendance/month/${year}/${month}`, {
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async getByEmployee(employeeId) {
        const res = await fetch(`${API_BASE}/api/attendance/employee/${employeeId}`, {
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async getByEmployeeMonthYear(employeeId, year, month) {
        const res = await fetch(`${API_BASE}/api/attendance/${employeeId}/${year}/${month}`, {
            headers: getHeaders()
        });
        return handleResponse(res);
    }
};

// ============================================
//  PAYROLL
// ============================================
const PayrollAPI = {
    async generate(data) {
        const res = await fetch(`${API_BASE}/api/payroll/generate`, {
            method: "POST",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return handleResponse(res);
    },

    async getById(id) {
        const res = await fetch(`${API_BASE}/api/payroll/${id}`, {
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async getByMonthYear(month, year) {
        const res = await fetch(`${API_BASE}/api/payroll/by-month/${year}/${month}`, {
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async getByEmployee(employeeId) {
        const res = await fetch(`${API_BASE}/api/payroll/by-employee/${employeeId}`, {
            headers: getHeaders()
        });
        return handleResponse(res);
    },

    async updateDeduction(id, deduction, incentive = 0) {
    const res = await fetch(`${API_BASE}/api/payroll/${id}/deduction`, {
        method: "PUT",
        headers: getHeaders(),
        body: JSON.stringify({ deduction, incentive })
    });
    return handleResponse(res);
},

    async markAsPaid(id) {
        const res = await fetch(`${API_BASE}/api/payroll/${id}/mark-paid`, {
            method: "PATCH",
            headers: getHeaders()
        });
        return handleResponse(res);
    }
};

// ============================================
//  PROFILE (Employee)
// ============================================
const ProfileAPI = {
    async get() {
        const res = await fetch(`${API_BASE}/api/profile`, { headers: getHeaders() });
        return handleResponse(res);
    },

    async update(data) {
        const res = await fetch(`${API_BASE}/api/profile`, {
            method: "PUT",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return handleResponse(res);
    },

    async getPayslips() {
        const res = await fetch(`${API_BASE}/api/profile/payslips`, { headers: getHeaders() });
        return handleResponse(res);
    },

    async getAttendance() {
        const res = await fetch(`${API_BASE}/api/profile/attendance`, { headers: getHeaders() });
        return handleResponse(res);
    }
};

// ============================================
//  DASHBOARD
// ============================================
const DashboardAPI = {
    async getAdmin() {
        const res = await fetch(`${API_BASE}/api/dashboard/admin`, { headers: getHeaders() });
        return handleResponse(res);
    }
};

// ============================================
//  SETTINGS
// ============================================
const SettingsAPI = {
    async get() {
        const res = await fetch(`${API_BASE}/api/settings`, { headers: getHeaders() });
        return handleResponse(res);
    },

    async update(data) {
        const res = await fetch(`${API_BASE}/api/settings`, {
            method: "PUT",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return handleResponse(res);
    },

    async uploadLogo(file) {
        const formData = new FormData();
        formData.append("file", file);
        const res = await fetch(`${API_BASE}/api/settings/upload-logo`, {
            method: "POST",
            headers: { "Authorization": `Bearer ${getToken()}` },
            body: formData
        });
        return handleResponse(res);
    }
};
// ============================================
//  BRANCH SETTINGS LOADER
// ============================================
async function loadBranchSettings() {
    try {
        const res = await fetch(`${API_BASE}/api/settings`);
        const data = await res.json();
        
        const nameEl = document.getElementById("school-name");
        if (nameEl && data.schoolName) nameEl.textContent = data.schoolName;

        const logoEl = document.getElementById("sidebar-logo");
        if (logoEl && data.logoPath) {
            logoEl.innerHTML = `<img src="${data.logoPath}?t=${Date.now()}" 
                style="width:100%;height:100%;object-fit:cover;border-radius:10px;">`;
        }
    } catch(e) {}

}