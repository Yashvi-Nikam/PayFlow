// ============================================
//  PayFlow - Auth Logic
// ============================================

document.addEventListener("DOMContentLoaded", () => {

    // If already logged in redirect to correct dashboard
    if (isLoggedIn()) {
        const role = getUser().role;
        if (role === "Admin") {
            window.location.href = "admin/dashboard.html";
        } else {
            window.location.href = "employee/dashboard.html";
        }
        return;
    }

    const loginForm = document.getElementById("login-form");
    const loginBtn = document.getElementById("login-btn");
    const loginError = document.getElementById("login-error");
    const togglePassword = document.getElementById("toggle-password");
    const passwordInput = document.getElementById("password");

    // Toggle password visibility
    if (togglePassword) {
        togglePassword.addEventListener("click", () => {
            const type = passwordInput.type === "password" ? "text" : "password";
            passwordInput.type = type;
            const icon = togglePassword.querySelector("i");
            icon.className = `fas fa-${type === "password" ? "eye" : "eye-slash"}`;
        });
    }

    // Login form submit
    if (loginForm) {
        loginForm.addEventListener("submit", async (e) => {
            e.preventDefault();

            const username = document.getElementById("username").value.trim();
            const password = document.getElementById("password").value;

            if (!username || !password) {
                showLoginError("Please enter username and password.");
                return;
            }

            loginBtn.disabled = true;
            loginBtn.innerHTML = `<span class="spinner-border spinner-border-sm me-2"></span>Signing in...`;

            try {
                const data = await AuthAPI.login(username, password);
                saveUser(data);

                // Check first login
                if (data.isFirstLogin) {
                    window.location.href = "changepassword.html";
                    return;
                }

                // Redirect by role
                if (data.role === "Admin") {
                    window.location.href = "admin/dashboard.html";
                } else {
                    window.location.href = "employee/dashboard.html";
                }

            } catch (err) {
                showLoginError(err.message || "Invalid username or password.");
                loginBtn.disabled = false;
                loginBtn.innerHTML = `<i class="fas fa-arrow-right me-2"></i>Sign In`;
            }
        });
    }

    function showLoginError(msg) {
        if (loginError) {
            loginError.textContent = msg;
            loginError.style.display = "flex";
        }
    }
});

