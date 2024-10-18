import React from 'react';
import { useEffect } from 'react';

const CurrentPortfolioValue = ({ currentValue, fetchCurrentValue }) => {
  const valueClass = currentValue?.toString().includes('-') ? 'value-negative' : 'value-positive';
  
  useEffect(() => {
    fetchCurrentValue();
  }, []);

  return (
    <div className='portfolio-infobox'>
      <p>Current Value:</p>
      <p className={valueClass}>${currentValue}</p>
    </div>
  )
}

export default CurrentPortfolioValue