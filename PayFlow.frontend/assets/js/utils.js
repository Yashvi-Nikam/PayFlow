// ============================================
//  PayFlow - Utility Functions
// ============================================

// ============================================
//  USER SESSION
// ============================================
function saveUser(data) {
    localStorage.setItem("payflow_token", data.token);
    localStorage.setItem("payflow_role", data.role);
    localStorage.setItem("payflow_username", data.username);
    localStorage.setItem("payflow_firstlogin", data.isFirstLogin);
}

function getUser() {
    return {
        token: localStorage.getItem("payflow_token"),
        role: localStorage.getItem("payflow_role"),
        username: localStorage.getItem("payflow_username"),
        firstLogin: localStorage.getItem("payflow_firstlogin") === "true"
    };
}

function clearUser() {
    localStorage.removeItem("payflow_token");
    localStorage.removeItem("payflow_role");
    localStorage.removeItem("payflow_username");
    localStorage.removeItem("payflow_firstlogin");
}

function isLoggedIn() {
    return !!localStorage.getItem("payflow_token");
}

function requireAuth() {
    if (!isLoggedIn()) {
        window.location.href = getBasePath() + "index.html";
    }
}

function requireAdmin() {
    requireAuth();
    if (getUser().role !== "Admin") {
        window.location.href = getBasePath() + "employee/dashboard.html";
    }
}

function requireEmployee() {
    requireAuth();
    if (getUser().role !== "Employee") {
        window.location.href = getBasePath() + "admin/dashboard.html";
    }
}

function getBasePath() {
    const path = window.location.pathname;
    if (path.includes("/admin/") || path.includes("/employee/")) {
        return "../";
    }
    return "";
}

function logout() {
    clearUser();
    window.location.href = getBasePath() + "index.html";
}

// ============================================
//  DATE & TIME
// ============================================
const MONTHS = [
    "January", "February", "March", "April",
    "May", "June", "July", "August",
    "September", "October", "November", "December"
];

function getCurrentMonth() { return new Date().getMonth() + 1; }
function getCurrentYear() { return new Date().getFullYear(); }

function getMonthName(month) { return MONTHS[month - 1]; }

function formatDate(dateStr) {
    if (!dateStr) return "—";
    const d = new Date(dateStr);
    return d.toLocaleDateString("en-IN", { day: "2-digit", month: "short", year: "numeric" });
}

function formatMonthYear(month, year) {
    return `${getMonthName(month)} ${year}`;
}

// ============================================
//  CURRENCY
// ============================================
function formatCurrency(amount) {
    if (amount == null) return "₹0";
    return "₹" + Number(amount).toLocaleString("en-IN");
}

// ============================================
//  INITIALS AVATAR
// ============================================
function getInitials(name) {
    if (!name) return "?";
    return name.split(" ").map(n => n[0]).join("").toUpperCase().slice(0, 2);
}

function getAvatarColor(name) {
    const colors = ["#6c63ff", "#22c55e", "#3b82f6", "#f59e0b", "#ef4444", "#8b5cf6"];
    let hash = 0;
    for (let c of (name || "")) hash += c.charCodeAt(0);
    return colors[hash % colors.length];
}

function createAvatar(name, photoPath, size = 36) {
    if (photoPath) {
        return `<img src="${photoPath}" 
                     style="width:${size}px;height:${size}px;border-radius:50%;object-fit:cover;" 
                     onerror="this.outerHTML=createAvatarFallback('${name}', ${size})">`;
    }
    return createAvatarFallback(name, size);
}

function createAvatarFallback(name, size = 36) {
    const color = getAvatarColor(name);
    return `<div style="width:${size}px;height:${size}px;border-radius:50%;
                        background:${color};display:flex;align-items:center;
                        justify-content:center;font-size:${size * 0.35}px;
                        font-weight:700;color:white;flex-shrink:0;">
                ${getInitials(name)}
            </div>`;
}

// ============================================
//  TOAST NOTIFICATIONS
// ============================================
function showToast(message, type = "success") {
    const existing = document.getElementById("toast-container");
    if (existing) existing.remove();

    const colors = {
        success: "#22c55e",
        error: "#ef4444",
        warning: "#f59e0b",
        info: "#3b82f6"
    };

    const icons = {
        success: "fa-check-circle",
        error: "fa-times-circle",
        warning: "fa-exclamation-circle",
        info: "fa-info-circle"
    };

    const toast = document.createElement("div");
    toast.id = "toast-container";
    toast.innerHTML = `
        <div style="
            position: fixed; bottom: 24px; right: 24px; z-index: 9999;
            background: #1e2130; border: 1px solid ${colors[type]};
            border-radius: 12px; padding: 14px 20px;
            display: flex; align-items: center; gap: 10px;
            box-shadow: 0 8px 32px rgba(0,0,0,0.4);
            animation: slideIn 0.3s ease;
            max-width: 360px;
        ">
            <i class="fas ${icons[type]}" style="color:${colors[type]};font-size:18px;"></i>
            <span style="color:#fff;font-size:14px;font-weight:500;">${message}</span>
        </div>
        <style>
            @keyframes slideIn {
                from { transform: translateX(100px); opacity: 0; }
                to   { transform: translateX(0); opacity: 1; }
            }
        </style>
    `;
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3500);
}

// ============================================
//  TOPBAR SETUP
// ============================================
function setupTopbar(activePage) {
    const user = getUser();
    const usernameEl = document.getElementById("topbar-username");
    const roleEl = document.getElementById("topbar-role");
    const avatarEl = document.getElementById("topbar-avatar");

    if (usernameEl) usernameEl.textContent = user.username || "User";
    if (roleEl) roleEl.textContent = user.role || "";
    if (avatarEl) avatarEl.innerHTML = getInitials(user.username);
}

// ============================================
//  CURRENT MONTH DISPLAY
// ============================================
function setupCurrentMonth() {
    const el = document.getElementById("current-month-display");
    if (el) el.textContent = `${getMonthName(getCurrentMonth())} ${getCurrentYear()}`;

    const dateEl = document.getElementById("today-date");
    if (dateEl) dateEl.textContent = `Today is ${formatDate(new Date())}`;
}

// ============================================
//  EMPLOYEE ID FORMAT
// ============================================
function formatEmployeeId(id) {
    return "EMP" + String(id).padStart(3, "0");
}

// ============================================
//  CONFIRMATION DIALOG
// ============================================
function confirmAction(message) {
    return confirm(message);
}

