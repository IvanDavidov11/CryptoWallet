import React from 'react';
import { useEffect } from 'react';

const CurrentPortfolioValue = ({ currentValue, fetchCurrentValue }) => {
  const valueClass = currentValue?.toString().includes('-') ? 'value-negative' : 'value-positive';
  
  useEffect(() => {
    fetchCurrentValue();
  }, []);

  return (
    <div className='portfolio'>
      <h3>Current Value:</h3>
      <h4 className={valueClass}>${currentValue}</h4>
    </div>
  )
}

export default CurrentPortfolioValue