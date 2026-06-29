import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import './HomePage.css'
import { KeyCloakConfig } from '../statics/configuration'

export default function HomePage() {
  const { isLoggedIn, login } = useAuth()
  const navigate = useNavigate()

  function handleLogin() {
	const returnUrl = encodeURIComponent(window.location.origin + '/dashboard');
	const keycloakUrl = `${import.meta.env.VITE_API_BASE}/api/auth/login?returnUrl=${returnUrl}`;
	console.log("LoginUrl", keycloakUrl)
    	login()
	window.location.href = keycloakUrl;
    // navigate('/dashboard')
  }

  return (
    <main className="main-content">
      <h1>Combine multiple video streams into one</h1>
      <p className="tagline">
        Bring live video from your cameras, friends, or remote guests directly into a single composited output. 100% free and open-source.
      </p>
      {!isLoggedIn && (
        <button onClick={handleLogin} className="cta-btn">Login</button>
      )}
    </main>
  )
}
