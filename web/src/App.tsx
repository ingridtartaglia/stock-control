import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Container, Button, Box } from '@mui/material';
import { StockMovementForm } from './components/StockMovement';
import { StockReportView } from './components/StockReport';
import { useTranslation } from 'react-i18next';

function App() {
  const { t } = useTranslation();

  return (
    <Router>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            {t('common.stock-control')}
          </Typography>
          <Button color="inherit" component={Link} to="/">
            {t('common.movement')}
          </Button>
          <Button color="inherit" component={Link} to="/report">
            {t('common.report')}
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