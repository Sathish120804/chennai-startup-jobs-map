import React from 'react';
import { cn } from '../../utils/cn';

export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  leftIcon?: React.ReactNode;
  rightIcon?: React.ReactNode;
}

export const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, leftIcon, rightIcon, ...props }, ref) => {
    return (
      <div className="relative flex items-center w-full">
        {leftIcon && (
          <div className="absolute left-3.5 text-slate-400 pointer-events-none flex items-center">
            {leftIcon}
          </div>
        )}
        <input
          ref={ref}
          className={cn(
            'w-full rounded-lg border border-slate-200 bg-white text-sm text-slate-900 placeholder:text-slate-400',
            'transition-colors duration-150',
            'focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20',
            leftIcon ? 'pl-10' : 'pl-3.5',
            rightIcon ? 'pr-10' : 'pr-3.5',
            'py-2',
            className
          )}
          {...props}
        />
        {rightIcon && (
          <div className="absolute right-3.5 text-slate-400 flex items-center">
            {rightIcon}
          </div>
        )}
      </div>
    );
  }
);

Input.displayName = 'Input';
