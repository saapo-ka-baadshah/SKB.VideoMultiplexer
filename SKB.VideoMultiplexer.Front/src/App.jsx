import { lazy, Suspense } from 'react'
import { Routes, Route } from 'react-router-dom'
import Navbar from './components/Navbar'
import ProtectedRoute from './components/ProtectedRoute'
import LoadingFallback from './components/LoadingFallback'
import HomePage from './pages/HomePage'
import './App.css'

const DashboardPage = lazy(() => import('./pages/DashboardPage'))

function App() {
  return (
    <div className="app">
      <Navbar />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/dashboard" element={
          <Suspense fallback={<LoadingFallback />}>
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          </Suspense>
        } />
      </Routes>
    </div>
  )
}

export default App
