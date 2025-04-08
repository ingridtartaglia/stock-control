import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Container, Button, Box } from '@mui/material';
import { StockMovementForm } from './components/StockMovement';
import { StockReportView } from './components/StockReport';

function App() {
  return (
    <Router>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Controle de Estoque
          </Typography>
          <Button color="inherit" component={Link} to="/">
            Movimentação
          </Button>
          <Button color="inherit" component={Link} to="/report">
            Relatório
          </Button>
        </Toolbar>
      </AppBar>

      <Container>
        <Box sx={{ mt: 4 }}>
          <Routes>
            <Route path="/" element={<StockMovementForm />} />
            <Route path="/report" element={<StockReportView />} />
          </Routes>
        </Box>
      </Container>
    </Router>
  );
}

export default App;