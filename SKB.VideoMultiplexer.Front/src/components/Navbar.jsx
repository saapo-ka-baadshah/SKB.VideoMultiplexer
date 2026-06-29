import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import './Navbar.css'

export default function Navbar() {
  const { isLoggedIn } = useAuth()

  return (
    <nav className="navbar">
      <Link to="/" className="nav-brand">SKB.VideoMultiplexer</Link>
      {isLoggedIn && (
        <Link to="/dashboard" className="nav-link">Dashboard</Link>
      )}
    </nav>
  )
}
