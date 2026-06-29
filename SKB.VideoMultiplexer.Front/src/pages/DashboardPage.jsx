import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import './DashboardPage.css'

export default function DashboardPage() {
  const { logout } = useAuth()
  const navigate = useNavigate()

  function handleLogout() {
    logout()
    navigate('/')
  }

  return (
    <div className="dashboard">
      <header className="dashboard-header">
        <h1>Welcome back!</h1>
        <p className="dashboard-subtitle">You are logged in. Here is an overview of your streams.</p>
      </header>

      <div className="dashboard-grid">
        <div className="dashboard-card">
          <h3>Active Streams</h3>
          <p className="card-stat">3</p>
          <p className="card-desc">Currently running compositions</p>
        </div>
        <div className="dashboard-card">
          <h3>Total Sources</h3>
          <p className="card-stat">12</p>
          <p className="card-desc">Cameras and media inputs</p>
        </div>
        <div className="dashboard-card">
          <h3>Recent Outputs</h3>
          <p className="card-stat">28</p>
          <p className="card-desc">Compositions in the last 7 days</p>
        </div>
      </div>

      <button onClick={handleLogout} className="logout-link">Logout</button>
    </div>
  )
}
